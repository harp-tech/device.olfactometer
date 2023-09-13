#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "aux_funcs.h"

#define F_CPU 32000000 //need to be defined for delay.h
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);
countdown_t pulse_countdown;
status_PWM_DC_t status_DC;

uint16_t ADC_sampling_counter = 0;
#define ADC_SAMPLING_DIVIDER 2 //2*1ms
#define CLOSE_LOOP_TIMING 5 //2*1*5ms 
uint16_t close_loop_counter_ms = 0;
uint8_t close_loop_case = 0;
uint8_t calibration_size = 11; //number of array positions for calibration  

uint16_t CH100_flows [] = {0, 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 100};
uint16_t CH1000_flows [] = {0, 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1000};
	
uint16_t CH0_calibration_values [] = {3259, 3819, 4336, 4824, 5284, 5722, 6144, 6549, 6928, 7283, 7636};
uint16_t CH1_calibration_values [] = {3259, 3819, 4336, 4824, 5284, 5722, 6144, 6549, 6928, 7283, 7636};
uint16_t CH2_calibration_values [] = {3259, 3819, 4336, 4824, 5284, 5722, 6144, 6549, 6928, 7283, 7636};
uint16_t CH3_calibration_values [] = {3259, 3819, 4336, 4824, 5284, 5722, 6144, 6549, 6928, 7283, 7636};
uint16_t CH4_calibration_values [] = {3391, 5176, 6389, 7357, 8166, 8872, 9493, 10060, 10554, 11006, 11430};
uint16_t CH3_calibration_aux_values [] = {3391, 5176, 6389, 7357, 8166, 8872, 9493, 10060, 10554, 11006, 11430};

/************************************************************************/
/* User functions                                                       */
/************************************************************************/

void init_calibration_values(void);
void closed_loop_control(uint8_t flow);
float interpolate_aux(uint16_t inValue, uint16_t lower_x, uint16_t upper_x, uint16_t lower_y, uint16_t upper_y);


/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "Olfactometer";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 1;
    uint8_t hwL = 0;
    uint8_t fwH = 1;
    uint8_t fwL = 0;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        1140,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name,
		false,	// The device is _not_ able to repeat the harp timestamp clock
		false,	// The device is _not_ able to generate the harp timestamp clock
		0		// Default timestamp offset
    );
	
	//init_calibration_values();

}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	//stop timers and clear ports
	timer_type0_stop(&TCC0);
	timer_type0_stop(&TCD0);
	timer_type0_stop(&TCE0);
	timer_type0_stop(&TCF0);
	timer_type1_stop(&TCD1);
	
	clr_VALVE0;
	clr_VALVE1;
	clr_VALVE2;
	clr_VALVE3;
	clr_ENDVALVE0;
	clr_ENDVALVE1;
	clr_DUMMYVALVE;
	
	clr_OUT0;
	clr_OUT1;
	
}



/************************************************************************/
/* Read calibration values from EEPROM                                  */
/************************************************************************/

void init_calibration_values(void)
{
	
	uint16_t index0 = 1600;
	uint16_t index = index0;
	
	index = index0;
	CH0_calibration_values[0] = eeprom_rd_byte(index);
	CH0_calibration_values[0] = ((CH0_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[1] = eeprom_rd_byte(++index);
	CH0_calibration_values[1] = ((CH0_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[2] = eeprom_rd_byte(++index);
	CH0_calibration_values[2] = ((CH0_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[3] = eeprom_rd_byte(++index);
	CH0_calibration_values[3] = ((CH0_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[4] = eeprom_rd_byte(++index);
	CH0_calibration_values[4] = ((CH0_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[5] = eeprom_rd_byte(++index);
	CH0_calibration_values[5] = ((CH0_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[6] = eeprom_rd_byte(++index);
	CH0_calibration_values[6] = ((CH0_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[7] = eeprom_rd_byte(++index);
	CH0_calibration_values[7] = ((CH0_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[8] = eeprom_rd_byte(++index);
	CH0_calibration_values[8] = ((CH0_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[9] = eeprom_rd_byte(++index);
	CH0_calibration_values[9] = ((CH0_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH0_calibration_values[10] = eeprom_rd_byte(++index);
	CH0_calibration_values[10] = ((CH0_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	
	index = index0 + 32;
	CH1_calibration_values[0] = eeprom_rd_byte(index);
	CH1_calibration_values[0] = ((CH1_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[1] = eeprom_rd_byte(++index);
	CH1_calibration_values[1] = ((CH1_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[2] = eeprom_rd_byte(++index);
	CH1_calibration_values[2] = ((CH1_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[3] = eeprom_rd_byte(++index);
	CH1_calibration_values[3] = ((CH1_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[4] = eeprom_rd_byte(++index);
	CH1_calibration_values[4] = ((CH1_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[5] = eeprom_rd_byte(++index);
	CH1_calibration_values[5] = ((CH1_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[6] = eeprom_rd_byte(++index);
	CH1_calibration_values[6] = ((CH1_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[7] = eeprom_rd_byte(++index);
	CH1_calibration_values[7] = ((CH1_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[8] = eeprom_rd_byte(++index);
	CH1_calibration_values[8] = ((CH1_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[9] = eeprom_rd_byte(++index);
	CH1_calibration_values[9] = ((CH1_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH1_calibration_values[10] = eeprom_rd_byte(++index);
	CH1_calibration_values[10] = ((CH1_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);	
	
	index = index0 + 64;
	CH2_calibration_values[0] = eeprom_rd_byte(index);
	CH2_calibration_values[0] = ((CH2_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[1] = eeprom_rd_byte(++index);
	CH2_calibration_values[1] = ((CH2_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[2] = eeprom_rd_byte(++index);
	CH2_calibration_values[2] = ((CH2_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[3] = eeprom_rd_byte(++index);
	CH2_calibration_values[3] = ((CH2_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[4] = eeprom_rd_byte(++index);
	CH2_calibration_values[4] = ((CH2_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[5] = eeprom_rd_byte(++index);
	CH2_calibration_values[5] = ((CH2_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[6] = eeprom_rd_byte(++index);
	CH2_calibration_values[6] = ((CH2_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[7] = eeprom_rd_byte(++index);
	CH2_calibration_values[7] = ((CH2_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[8] = eeprom_rd_byte(++index);
	CH2_calibration_values[8] = ((CH2_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[9] = eeprom_rd_byte(++index);
	CH2_calibration_values[9] = ((CH2_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH2_calibration_values[10] = eeprom_rd_byte(++index);
	CH2_calibration_values[10] = ((CH2_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	
	if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
		index = index0 + 96;
		CH3_calibration_values[0] = eeprom_rd_byte(index);
		CH3_calibration_values[0] = ((CH3_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[1] = eeprom_rd_byte(++index);
		CH3_calibration_values[1] = ((CH3_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[2] = eeprom_rd_byte(++index);
		CH3_calibration_values[2] = ((CH3_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[3] = eeprom_rd_byte(++index);
		CH3_calibration_values[3] = ((CH3_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[4] = eeprom_rd_byte(++index);
		CH3_calibration_values[4] = ((CH3_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[5] = eeprom_rd_byte(++index);
		CH3_calibration_values[5] = ((CH3_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[6] = eeprom_rd_byte(++index);
		CH3_calibration_values[6] = ((CH3_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[7] = eeprom_rd_byte(++index);
		CH3_calibration_values[7] = ((CH3_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[8] = eeprom_rd_byte(++index);
		CH3_calibration_values[8] = ((CH3_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[9] = eeprom_rd_byte(++index);
		CH3_calibration_values[9] = ((CH3_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[10] = eeprom_rd_byte(++index);
		CH3_calibration_values[10] = ((CH3_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	}
	
	index = index0 + 128;
	CH4_calibration_values[0] = eeprom_rd_byte(index);
	CH4_calibration_values[0] = ((CH4_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[1] = eeprom_rd_byte(++index);
	CH4_calibration_values[1] = ((CH4_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[2] = eeprom_rd_byte(++index);
	CH4_calibration_values[2] = ((CH4_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[3] = eeprom_rd_byte(++index);
	CH4_calibration_values[3] = ((CH4_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[4] = eeprom_rd_byte(++index);
	CH4_calibration_values[4] = ((CH4_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[5] = eeprom_rd_byte(++index);
	CH4_calibration_values[5] = ((CH4_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[6] = eeprom_rd_byte(++index);
	CH4_calibration_values[6] = ((CH4_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[7] = eeprom_rd_byte(++index);
	CH4_calibration_values[7] = ((CH4_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[8] = eeprom_rd_byte(++index);
	CH4_calibration_values[8] = ((CH4_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[9] = eeprom_rd_byte(++index);
	CH4_calibration_values[9] = ((CH4_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	CH4_calibration_values[10] = eeprom_rd_byte(++index);
	CH4_calibration_values[10] = ((CH4_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	
	
	if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_1000){
		index = index0 + 160;
		CH3_calibration_values[0] = eeprom_rd_byte(index);
		CH3_calibration_values[0] = ((CH3_calibration_values[0] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[1] = eeprom_rd_byte(++index);
		CH3_calibration_values[1] = ((CH3_calibration_values[1] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[2] = eeprom_rd_byte(++index);
		CH3_calibration_values[2] = ((CH3_calibration_values[2] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[3] = eeprom_rd_byte(++index);
		CH3_calibration_values[3] = ((CH3_calibration_values[3] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[4] = eeprom_rd_byte(++index);
		CH3_calibration_values[4] = ((CH3_calibration_values[4] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[5] = eeprom_rd_byte(++index);
		CH3_calibration_values[5] = ((CH3_calibration_values[5] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[6] = eeprom_rd_byte(++index);
		CH3_calibration_values[6] = ((CH3_calibration_values[6] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[7] = eeprom_rd_byte(++index);
		CH3_calibration_values[7] = ((CH3_calibration_values[7] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[8] = eeprom_rd_byte(++index);
		CH3_calibration_values[8] = ((CH3_calibration_values[8] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[9] = eeprom_rd_byte(++index);
		CH3_calibration_values[9] = ((CH3_calibration_values[9] << 8) & 0xFF00) | eeprom_rd_byte(++index);
		CH3_calibration_values[10] = eeprom_rd_byte(++index);
		CH3_calibration_values[10] = ((CH3_calibration_values[10] << 8) & 0xFF00) | eeprom_rd_byte(++index);
	}

}

/************************************************************************/
/* Closed Loop Control - aux Interpolate function                       */
/************************************************************************/

float interpolate_aux(uint16_t inValue, uint16_t lower_x, uint16_t upper_x, uint16_t lower_y, uint16_t upper_y)
{
	float slope = (float)(upper_y - lower_y) / (upper_x - lower_x) * 1.0;
	float interpolated = (float)(inValue-lower_x) * slope + lower_y * 1.0;
	
	if (interpolated < lower_y){
		return lower_y*1.0;
	}
	if (interpolated > upper_y){
		return upper_y*1.0;
	}
	return interpolated; 
}



/************************************************************************/
/* Closed Loop Control                                                  */
/************************************************************************/

void closed_loop_control(uint8_t flow)
{
	
	/* Takes 1100 us */ //re-check!!!!!!!!!!!!!!!!!!!!!!!
		
	uint16_t calibration_values [] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,32768,500};
	uint16_t calibration_values_1000 [] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,32768,3000};
	float flow_real = 0;
	float calc_dutycycle = 0;
	float low_limit_dc = 1.0;
	float high_limit_dc = 99.0;
	float error = 0;
	
	uint8_t flowmeter = flow;
	uint8_t index = 0;
		
	//set_OUT0;  // test timing
	
	uint8_t user_calibration = 0;
	user_calibration = app_regs.REG_USER_CALIBRATION_ENABLE;
	
	switch (flowmeter)
	{
		case 0: 
			flow_real = app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[0]; // raw ADC analog output signal [2^16]
			
			// create calibration array
			index = 0;
			while(index < calibration_size){
				if(!user_calibration){
					calibration_values[index*2+2] = CH0_calibration_values[index];
					calibration_values[index*2+3] = CH100_flows[index+1];
				}
				else{
					calibration_values[index*2+2] = app_regs.REG_CHANNEL0_USER_CALIBRATION[index];
					calibration_values[index*2+3] = CH100_flows[index+1];
				}
				index = index + 1;
			}
				
			// determine real flow rate by interpolation of calibration values
			index = 0;
			while(!(flow_real < calibration_values[index])){
				index = index + 2;
			}
			flow_real = interpolate_aux(flow_real, calibration_values[index-2], calibration_values[index], calibration_values[index-1], calibration_values[index+1]);
			
			app_regs.REG_CHANNEL0_FLOW_REAL = flow_real;
			if (app_regs.REG_ENABLE_EVENTS & B_EVT2){
				core_func_send_event(ADD_REG_CHANNEL0_FLOW_REAL, true);
			}
			
			// P control
			error = (app_regs.REG_CHANNEL0_FLOW_TARGET-app_regs.REG_CHANNEL0_FLOW_REAL);
			calc_dutycycle = app_regs.REG_CHANNEL0_DUTY_CYCLE + error/32; 
			
			if (calc_dutycycle <= 1) { app_write_REG_CHANNEL0_DUTY_CYCLE(&low_limit_dc); } else if (calc_dutycycle >= 99) {
				app_write_REG_CHANNEL0_DUTY_CYCLE(&high_limit_dc); } else { app_write_REG_CHANNEL0_DUTY_CYCLE(&calc_dutycycle); }
			break;
		
		
		case 1:
			flow_real = app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[1]; // raw analog output signal [2^16]
			
				// create calibration array
				index = 0;
				while(index < calibration_size){
					if(!user_calibration){
						calibration_values[index*2+2] = CH1_calibration_values[index];
						calibration_values[index*2+3] = CH100_flows[index+1];
					}
					else{
						calibration_values[index*2+2] = app_regs.REG_CHANNEL1_USER_CALIBRATION[index];
						calibration_values[index*2+3] = CH100_flows[index+1];
					}
					index = index + 1;
				}
									
			// determine real flow rate by interpolation of calibration values
			index = 0;
			while(!(flow_real < calibration_values[index])){
				index = index + 2;
			}
			flow_real = interpolate_aux(flow_real, calibration_values[index-2], calibration_values[index], calibration_values[index-1], calibration_values[index+1]);
			
			app_regs.REG_CHANNEL1_FLOW_REAL = flow_real;
			if (app_regs.REG_ENABLE_EVENTS & B_EVT2){
				core_func_send_event(ADD_REG_CHANNEL1_FLOW_REAL, true);
			}
						
			// P control
			error = (app_regs.REG_CHANNEL1_FLOW_TARGET-app_regs.REG_CHANNEL1_FLOW_REAL);
			calc_dutycycle = app_regs.REG_CHANNEL1_DUTY_CYCLE + error/32; 
			
			if (calc_dutycycle <= 1) { app_write_REG_CHANNEL1_DUTY_CYCLE(&low_limit_dc); } else if (calc_dutycycle >= 99) {
				app_write_REG_CHANNEL1_DUTY_CYCLE(&high_limit_dc); } else { app_write_REG_CHANNEL1_DUTY_CYCLE(&calc_dutycycle); }
			break;
		
		
		case 2:
			flow_real = app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[2]; // raw ADC analog output signal [2^16]
		
			// create calibration array
			index = 0;
			while(index < calibration_size){
				if(!user_calibration){
					calibration_values[index*2+2] = CH2_calibration_values[index];
					calibration_values[index*2+3] = CH100_flows[index+1];
				}
				else{
					calibration_values[index*2+2] = app_regs.REG_CHANNEL2_USER_CALIBRATION[index];
					calibration_values[index*2+3] = CH100_flows[index+1];
				}
				index = index + 1;
			}
		
			// determine real flow rate by interpolation of calibration values
			index = 0;
			while(!(flow_real < calibration_values[index])){
				index = index + 2;
			}
			flow_real = interpolate_aux(flow_real, calibration_values[index-2], calibration_values[index], calibration_values[index-1], calibration_values[index+1]);
				
			app_regs.REG_CHANNEL2_FLOW_REAL = flow_real;
			if (app_regs.REG_ENABLE_EVENTS & B_EVT2){
				core_func_send_event(ADD_REG_CHANNEL2_FLOW_REAL, true);
			}
		
			// P control
			error = (app_regs.REG_CHANNEL2_FLOW_TARGET-app_regs.REG_CHANNEL2_FLOW_REAL);
			calc_dutycycle = app_regs.REG_CHANNEL2_DUTY_CYCLE + error/32; 
						
			if (calc_dutycycle <= 1) { app_write_REG_CHANNEL2_DUTY_CYCLE(&low_limit_dc); } else if (calc_dutycycle >= 99) {
				app_write_REG_CHANNEL2_DUTY_CYCLE(&high_limit_dc); } else { app_write_REG_CHANNEL2_DUTY_CYCLE(&calc_dutycycle); }
			break;
		
				
		case 3:
			flow_real = app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[3]; // raw ADC analog output signal [2^16]
			
			// create calibration array
			index = 0;
			while(index < calibration_size){
				if(!user_calibration){
					if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
						calibration_values[index*2+2] = CH3_calibration_values[index];
						calibration_values[index*2+3] = CH100_flows[index+1];
					}
					else{
						calibration_values_1000[index*2+2] = CH3_calibration_values[index];
						calibration_values_1000[index*2+3] = CH1000_flows[index+1];
					}
					
					
				}
				else{
					if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
						calibration_values[index*2+2] = app_regs.REG_CHANNEL3_USER_CALIBRATION[index];
						calibration_values[index*2+3] = CH100_flows[index+1];
					}
					else{
						calibration_values_1000[index*2+2] = app_regs.REG_CHANNEL3_USER_CALIBRATION_AUX[index];
						calibration_values_1000[index*2+3] = CH1000_flows[index+1];
					}
				}
				index = index + 1;
			}
		
			// determine real flow rate by interpolation of calibration values
			index = 0;
			while(!(flow_real < calibration_values[index])){
				index = index + 2;
			}
			
			if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
				// determine real flow rate by interpolation of calibration values
				index = 0;
				while(!(flow_real < calibration_values[index])){
					index = index + 2;
				}
				flow_real = interpolate_aux(flow_real, calibration_values[index-2], calibration_values[index], calibration_values[index-1], calibration_values[index+1]);
			}
			else {
				// determine real flow rate by interpolation of calibration values
				index = 0;
				while(!(flow_real < calibration_values_1000[index])){
					index = index + 2;
				}
				flow_real = interpolate_aux(flow_real, calibration_values_1000[index-2], calibration_values_1000[index], calibration_values_1000[index-1], calibration_values_1000[index+1]);
			
			}
			
			app_regs.REG_CHANNEL3_FLOW_REAL = flow_real;
			if (app_regs.REG_ENABLE_EVENTS & B_EVT2){
				core_func_send_event(ADD_REG_CHANNEL3_FLOW_REAL, true);
			}
		
			// P control
			error = (app_regs.REG_CHANNEL3_FLOW_TARGET-app_regs.REG_CHANNEL3_FLOW_REAL);
			calc_dutycycle = app_regs.REG_CHANNEL3_DUTY_CYCLE + error/256; 
						
			if (calc_dutycycle <= 1) { app_write_REG_CHANNEL3_DUTY_CYCLE(&low_limit_dc); } else if (calc_dutycycle >= 99) {
				app_write_REG_CHANNEL3_DUTY_CYCLE(&high_limit_dc); } else { app_write_REG_CHANNEL3_DUTY_CYCLE(&calc_dutycycle); }
			break;
		
			
		case 4: // flow meter 1000ml/min 
			flow_real = app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[4]; // raw analog output signal [2^16]
			
			// create calibration array
			index = 0;
			while(index < calibration_size){
				if(!user_calibration){
					calibration_values_1000[index*2+2] = CH4_calibration_values[index];
					calibration_values_1000[index*2+3] = CH1000_flows[index+1];
				}
				else{
					calibration_values_1000[index*2+2] = app_regs.REG_CHANNEL4_USER_CALIBRATION[index];
					calibration_values_1000[index*2+3] = CH1000_flows[index+1];
				}
				index = index + 1;
			}
			
			// determine real flow rate by interpolation of calibration values
			index = 0;
			while(!(flow_real < calibration_values_1000[index])){
				index = index + 2;
			}
			flow_real = interpolate_aux(flow_real, calibration_values_1000[index-2], calibration_values_1000[index], calibration_values_1000[index-1], calibration_values_1000[index+1]);
				
			app_regs.REG_CHANNEL4_FLOW_REAL = flow_real;
			if (app_regs.REG_ENABLE_EVENTS & B_EVT2){
				core_func_send_event(ADD_REG_CHANNEL4_FLOW_REAL, true);
			}
		
			// P control
			error = (app_regs.REG_CHANNEL4_FLOW_TARGET-app_regs.REG_CHANNEL4_FLOW_REAL);
			calc_dutycycle = app_regs.REG_CHANNEL4_DUTY_CYCLE + error/256; 
	
			if (calc_dutycycle <= 1) { app_write_REG_CHANNEL4_DUTY_CYCLE(&low_limit_dc); } else if (calc_dutycycle >= 99) {
				app_write_REG_CHANNEL4_DUTY_CYCLE(&high_limit_dc); } else { app_write_REG_CHANNEL4_DUTY_CYCLE(&calc_dutycycle); }
			break;
	}
	
}


/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_define_clock_default(void) {}

void core_callback_initialize_hardware(void){
	
	init_ios();
	
	init_calibration_values();
		
	/* Initialize SPI with 4MHz */
	SPIE_CTRL = SPI_MASTER_bm | SPI_ENABLE_bm | SPI_MODE_0_gc | SPI_CLK2X_bm | SPI_PRESCALER_DIV16_gc;
	
	/* Reset ADC */
	_delay_ms(100);
	set_RESET;
	_delay_ms(1);
	clr_RESET;
	_delay_ms(1);
	
	
}

void core_callback_1st_config_hw_after_boot(void)
{
	init_ios();
	
	init_calibration_values();
		
	/* Initialize SPI with 4MHz */
	SPIE_CTRL = SPI_MASTER_bm | SPI_ENABLE_bm | SPI_MODE_0_gc | SPI_CLK2X_bm | SPI_PRESCALER_DIV16_gc;
		
	/* Reset ADC */
	_delay_ms(100);
	set_RESET;
	_delay_ms(1);
	clr_RESET;
	_delay_ms(1);

	
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_ENABLE_FLOW = 0;
	
	app_regs.REG_USER_CALIBRATION_ENABLE = 0; 
			
	app_regs.REG_CHANNEL0_FLOW_TARGET = 50.0;
	app_regs.REG_CHANNEL0_FLOW_REAL = 0.0;
	app_regs.REG_CHANNEL1_FLOW_TARGET = 50.0;
	app_regs.REG_CHANNEL1_FLOW_REAL = 0.0;
	app_regs.REG_CHANNEL2_FLOW_TARGET = 50.0;
	app_regs.REG_CHANNEL2_FLOW_REAL = 0.0;
	app_regs.REG_CHANNEL3_FLOW_TARGET = 50.0;
	app_regs.REG_CHANNEL3_FLOW_REAL = 0.0;
	app_regs.REG_CHANNEL4_FLOW_TARGET = 50.0;
	app_regs.REG_CHANNEL4_FLOW_REAL = 0.0;
	
	app_regs.REG_CHANNEL0_FREQUENCY = 2500;
	app_regs.REG_CHANNEL0_DUTY_CYCLE = 1;
	app_regs.REG_CHANNEL1_FREQUENCY = 2500;
	app_regs.REG_CHANNEL1_DUTY_CYCLE = 1;
	app_regs.REG_CHANNEL2_FREQUENCY = 1500;
	app_regs.REG_CHANNEL2_DUTY_CYCLE = 1;
	app_regs.REG_CHANNEL3_FREQUENCY = 1500;
	app_regs.REG_CHANNEL3_DUTY_CYCLE = 1;
	app_regs.REG_CHANNEL4_FREQUENCY = 1500;
	app_regs.REG_CHANNEL4_DUTY_CYCLE = 1;
	
	app_regs.REG_ENABLE_VALVES_PULSE = 0;
	
	app_regs.REG_PULSE_VALVE0 = 500;
	app_regs.REG_PULSE_VALVE1 = 500;
	app_regs.REG_PULSE_VALVE2 = 500;
	app_regs.REG_PULSE_VALVE3 = 500;
	app_regs.REG_PULSE_ENDVALVE0 = 500;
	app_regs.REG_PULSE_ENDVALVE1 = 500;
	app_regs.REG_PULSE_DUMMYVALVE = 500;
	   
	app_regs.REG_ENABLE_EVENTS = B_EVT0 | B_EVT1 | B_EVT2;
	
	app_regs.REG_DI0_TRIGGER = GM_DIN0_SYNC;
	app_regs.REG_DO0_SYNC = GM_DOUT0_SOFTWARE;
	app_regs.REG_DO0_SYNC = GM_DOUT1_SOFTWARE;
	
	app_regs.REG_MIMIC_VALVE0 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_VALVE1 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_VALVE2 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_VALVE3 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_ENDVALVE0 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_ENDVALVE1 = GM_MIMIC_NONE;
	app_regs.REG_MIMIC_DUMMYVALVE = GM_MIMIC_NONE;
	
	app_regs.REG_CHANNEL3_RANGE = GM_FLOW_100;
	app_regs.REG_EXT_CTRL_VALVES = 0;
		
	status_DC.DC0_ready = 0;
	status_DC.DC1_ready = 0;
	status_DC.DC2_ready = 0;
	status_DC.DC3_ready = 0;
	status_DC.DC4_ready = 0;
	
	init_calibration_values();
	
}

void core_callback_registers_were_reinitialized(void)
{

	// duty cycle calculation update 
	hwbp_app_pwm_gen_update_dc0();
	hwbp_app_pwm_gen_update_dc1();
	hwbp_app_pwm_gen_update_dc2();
	hwbp_app_pwm_gen_update_dc3();
	hwbp_app_pwm_gen_update_dc4();

	// stop PWMs 
	timer_type0_stop(&TCC0);
	timer_type0_stop(&TCD0);
	timer_type0_stop(&TCE0);
	timer_type0_stop(&TCF0);
	timer_type1_stop(&TCD1);
	
	app_write_REG_DI0_TRIGGER(&app_regs.REG_DI0_TRIGGER);
	app_write_REG_DO0_SYNC(&app_regs.REG_DO0_SYNC);
	app_write_REG_DO1_SYNC(&app_regs.REG_DO1_SYNC);
	app_write_REG_MIMIC_VALVE0(&app_regs.REG_MIMIC_VALVE0);
	app_write_REG_MIMIC_VALVE1(&app_regs.REG_MIMIC_VALVE1);
	app_write_REG_MIMIC_VALVE2(&app_regs.REG_MIMIC_VALVE2);
	app_write_REG_MIMIC_VALVE3(&app_regs.REG_MIMIC_VALVE3);
	app_write_REG_MIMIC_ENDVALVE0(&app_regs.REG_MIMIC_ENDVALVE0);
	app_write_REG_MIMIC_ENDVALVE1(&app_regs.REG_MIMIC_ENDVALVE1);
	app_write_REG_MIMIC_DUMMYVALVE(&app_regs.REG_MIMIC_DUMMYVALVE);
	app_write_REG_CHANNEL3_RANGE(&app_regs.REG_CHANNEL3_RANGE);
		
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {
	
	app_regs.REG_ENABLE_FLOW = 0;
	app_write_REG_ENABLE_FLOW(&app_regs.REG_ENABLE_FLOW);
	
}

void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void) {}
void core_callback_t_500us(void) {
	
	if (pulse_countdown.valve0 > 0)
		if (--pulse_countdown.valve0 == 0)
			clr_VALVE0;
			
	if (pulse_countdown.valve1 > 0)
		if (--pulse_countdown.valve1 == 0)
			clr_VALVE1;
	
	if (pulse_countdown.valve2 > 0)
		if (--pulse_countdown.valve2 == 0)
			clr_VALVE2;
		
	if (pulse_countdown.valve3 > 0)
		if (--pulse_countdown.valve3 == 0)
			clr_VALVE3;
			
	if (pulse_countdown.valveaux0 > 0)
		if (--pulse_countdown.valveaux0 == 0)
			clr_ENDVALVE0;
	
	if (pulse_countdown.valveaux1 > 0)
		if (--pulse_countdown.valveaux1 == 0)
			clr_ENDVALVE1;
	
	if (pulse_countdown.valvedummy > 0)
		if (--pulse_countdown.valvedummy == 0)
			clr_DUMMYVALVE;
}

void core_callback_t_1ms(void) {

	// if flowmeter is running then each ms
	if (app_regs.REG_ENABLE_FLOW){
		core_func_mark_user_timestamp();
		
		// read ADC at 1ms x ADC_SAMPLING_DIVIDER
		if(++ADC_sampling_counter >= ADC_SAMPLING_DIVIDER){		
			set_CONVST;
			ADC_sampling_counter = 0;
			
			// go over each flow controller
			if(++close_loop_counter_ms >= CLOSE_LOOP_TIMING){
				if(++close_loop_case >= 5)
					close_loop_case = 0;
				closed_loop_control(close_loop_case);
				close_loop_counter_ms = 0;
			}				
		}
	}
	else {
	}
	
}


/************************************************************************/
/* Callbacks: clock control                                              */
/************************************************************************/
void core_callback_clock_to_repeater(void) {}
void core_callback_clock_to_generator(void) {}
void core_callback_clock_to_unlock(void) {}
void core_callback_clock_to_lock(void) {}
	
/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}
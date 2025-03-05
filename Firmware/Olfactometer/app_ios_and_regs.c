#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

//to be accessed in app_ios_regs.c
extern AppRegs app_regs; 

/************************************************************************/
/* Set the corresponding MIMIC functions                                */
/************************************************************************/
void mimic_valves (uint8_t reg, uint8_t function)
{
	switch (reg)
	{
		case GM_MIMIC_DO0:  
			if (function == _SET_IO_) set_OUT0;          
			if (function == _CLR_IO_) clr_OUT0;            
			if (function == _TGL_IO_) tgl_OUT0; 
			break;
		case GM_MIMIC_DO1:  
			if (function == _SET_IO_) set_OUT1;          
			if (function == _CLR_IO_) clr_OUT1;            
			if (function == _TGL_IO_) tgl_OUT1;
			break;
	}
}


/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	
	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // IN0
	io_pin2in(&PORTK, 7, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // VERSION_CTRL
	io_pin2in(&PORTE, 6, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // MISO
	io_pin2in(&PORTH, 1, PULL_IO_UP, SENSE_IO_EDGE_FALLING);             // BUSY
	io_pin2in(&PORTA, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE0CTRL
	io_pin2in(&PORTA, 1, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE1CTRL
	io_pin2in(&PORTA, 2, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE2CTRL
	io_pin2in(&PORTA, 3, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE3CTRL
	io_pin2in(&PORTK, 4, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE0CHKCTRL
	io_pin2in(&PORTK, 6, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE1CHKCTRL
	io_pin2in(&PORTK, 1, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE2CHKCTRL
	io_pin2in(&PORTK, 5, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // VALVE3CHKCTRL
	io_pin2in(&PORTA, 4, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // ENDVALVECTRL
	io_pin2in(&PORTA, 5, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // FLUSHVALVECTRL

	/* Configure input interrupts */
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // IN0
	io_set_int(&PORTH, INT_LEVEL_LOW, 0, (1<<1), false);                 // BUSY
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<0), false);                 // VALVE0CTRL
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<1), false);                 // VALVE1CTRL
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<2), false);                 // VALVE2CTRL
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<3), false);                 // VALVE3CTRL
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<4), false);                 // VALVE0CHKCTRL
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<6), false);                 // VALVE1CHKCTRL
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<1), false);                 // VALVE2CHKCTRL
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<5), false);                 // VALVE3CHKCTRL
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<4), false);                 // ENDVALVECTRL
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<5), false);                 // FLUSHVALVECTRL
	
	/* Configure output pins */
	io_pin2out(&PORTH, 4, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // OUT0
	io_pin2out(&PORTH, 5, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // OUT1
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // PWM0
	io_pin2out(&PORTD, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // PWM1
	io_pin2out(&PORTE, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // PWM2
	io_pin2out(&PORTF, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // PWM3
	io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // PWM4
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE0
	io_pin2out(&PORTD, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE1
	io_pin2out(&PORTE, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE2
	io_pin2out(&PORTF, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE3
	io_pin2out(&PORTD, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE0CHK
	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE1CHK
	io_pin2out(&PORTE, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE2CHK
	io_pin2out(&PORTF, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // VALVE3CHK
	io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RE_DE_5V_0
	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RE_DE_5V_1
	io_pin2out(&PORTB, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RE_DE_5V_2
	io_pin2out(&PORTB, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RE_DE_5V_3
	io_pin2out(&PORTB, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RE_DE_5V_4
	io_pin2out(&PORTE, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CS_ADC
	io_pin2out(&PORTF, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CONVST
	io_pin2out(&PORTE, 5, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // MOSI
	io_pin2out(&PORTE, 7, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // SCK
	io_pin2out(&PORTH, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RESET
	io_pin2out(&PORTJ, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DUMMY0
	io_pin2out(&PORTJ, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DUMMYVALVE
	io_pin2out(&PORTD, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ENDVALVE0
	io_pin2out(&PORTD, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ENDVALVE1
	io_pin2out(&PORTD, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // RANGE
	
	/* Initialize output pins */
	clr_OUT0;
	clr_OUT1;
	clr_PWM0;
	clr_PWM1;
	clr_PWM2;
	clr_PWM3;
	clr_PWM4;
	clr_VALVE0;
	clr_VALVE1;
	clr_VALVE2;
	clr_VALVE3;
	clr_VALVE0CHK;
	clr_VALVE1CHK;
	clr_VALVE2CHK;
	clr_VALVE3CHK;
	clr_RE_DE_5V_0;
	clr_RE_DE_5V_1;
	clr_RE_DE_5V_2;
	clr_RE_DE_5V_3;
	clr_RE_DE_5V_4;
	set_CS_ADC;
	clr_CONVST;
	clr_MOSI;
	clr_SCK;
	clr_RESET;
	clr_DUMMY0;
	clr_DUMMYVALVE;
	clr_ENDVALVE0;
	clr_ENDVALVE1;
	set_RANGE;

}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_I16,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_FLOAT,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	1,
	5,
	1,
	11,
	11,
	11,
	11,
	11,
	11,
	1,
	1,
	1,
	1,
	1,
	1,
	5,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_ENABLE_FLOW),
	(uint8_t*)(app_regs.REG_FLOWMETER_ANALOG_OUTPUTS),
	(uint8_t*)(&app_regs.REG_DI0_STATE),
	(uint8_t*)(app_regs.REG_CHANNEL0_USER_CALIBRATION),
	(uint8_t*)(app_regs.REG_CHANNEL1_USER_CALIBRATION),
	(uint8_t*)(app_regs.REG_CHANNEL2_USER_CALIBRATION),
	(uint8_t*)(app_regs.REG_CHANNEL3_USER_CALIBRATION),
	(uint8_t*)(app_regs.REG_CHANNEL4_USER_CALIBRATION),
	(uint8_t*)(app_regs.REG_CHANNEL3_USER_CALIBRATION_AUX),
	(uint8_t*)(&app_regs.REG_USER_CALIBRATION_ENABLE),
	(uint8_t*)(&app_regs.REG_CHANNEL0_TARGET_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL1_TARGET_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL2_TARGET_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL3_TARGET_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL4_TARGET_FLOW),
	(uint8_t*)(app_regs.REG_CHANNELS_TARGET_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL0_ACTUAL_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL1_ACTUAL_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL2_ACTUAL_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL3_ACTUAL_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL4_ACTUAL_FLOW),
	(uint8_t*)(&app_regs.REG_CHANNEL0_FREQUENCY),
	(uint8_t*)(&app_regs.REG_CHANNEL1_FREQUENCY),
	(uint8_t*)(&app_regs.REG_CHANNEL2_FREQUENCY),
	(uint8_t*)(&app_regs.REG_CHANNEL3_FREQUENCY),
	(uint8_t*)(&app_regs.REG_CHANNEL4_FREQUENCY),
	(uint8_t*)(&app_regs.REG_CHANNEL0_DUTY_CYCLE),
	(uint8_t*)(&app_regs.REG_CHANNEL1_DUTY_CYCLE),
	(uint8_t*)(&app_regs.REG_CHANNEL2_DUTY_CYCLE),
	(uint8_t*)(&app_regs.REG_CHANNEL3_DUTY_CYCLE),
	(uint8_t*)(&app_regs.REG_CHANNEL4_DUTY_CYCLE),
	(uint8_t*)(&app_regs.REG_OUTPUT_SET),
	(uint8_t*)(&app_regs.REG_OUTPUT_CLEAR),
	(uint8_t*)(&app_regs.REG_OUTPUT_TOGGLE),
	(uint8_t*)(&app_regs.REG_OUTPUT_STATE),
	(uint8_t*)(&app_regs.REG_ENABLE_VALVES_PULSE),
	(uint8_t*)(&app_regs.REG_VALVES_SET),
	(uint8_t*)(&app_regs.REG_VALVES_CLEAR),
	(uint8_t*)(&app_regs.REG_VALVES_TOGGLE),
	(uint8_t*)(&app_regs.REG_VALVES_STATE),
	(uint8_t*)(&app_regs.REG_ODOR_VALVES_STATE),
	(uint8_t*)(&app_regs.REG_END_VALVES_STATE),
	(uint8_t*)(&app_regs.REG_CHECK_VALVES_STATE),
	(uint8_t*)(&app_regs.REG_VALVE0_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_VALVE1_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_VALVE2_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_VALVE3_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_VALVE0CHK_DELAY),
	(uint8_t*)(&app_regs.REG_VALVE1CHK_DELAY),
	(uint8_t*)(&app_regs.REG_VALVE2CHK_DELAY),
	(uint8_t*)(&app_regs.REG_VALVE3CHK_DELAY),
	(uint8_t*)(&app_regs.REG_END_VALVE0_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_END_VALVE1_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_DUMMY_VALVE_PULSE_DURATION),
	(uint8_t*)(&app_regs.REG_DO0_SYNC),
	(uint8_t*)(&app_regs.REG_DO1_SYNC),
	(uint8_t*)(&app_regs.REG_DI0_TRIGGER),
	(uint8_t*)(&app_regs.REG_MIMIC_ODOR_VALVE0),
	(uint8_t*)(&app_regs.REG_MIMIC_ODOR_VALVE1),
	(uint8_t*)(&app_regs.REG_MIMIC_ODOR_VALVE2),
	(uint8_t*)(&app_regs.REG_MIMIC_ODOR_VALVE3),
	(uint8_t*)(&app_regs.REG_MIMIC_CHECK_VALVE0),
	(uint8_t*)(&app_regs.REG_MIMIC_CHECK_VALVE1),
	(uint8_t*)(&app_regs.REG_MIMIC_CHECK_VALVE2),
	(uint8_t*)(&app_regs.REG_MIMIC_CHECK_VALVE3),
	(uint8_t*)(&app_regs.REG_MIMIC_END_VALVE0),
	(uint8_t*)(&app_regs.REG_MIMIC_END_VALVE1),
	(uint8_t*)(&app_regs.REG_MIMIC_DUMMY_VALVE),
	(uint8_t*)(&app_regs.REG_ENABLE_VALVE_EXT_CTRL),
	(uint8_t*)(&app_regs.REG_CHANNEL3_RANGE),
	(uint8_t*)(&app_regs.REG_CHECK_VALVES_CTRL),
	(uint8_t*)(&app_regs.REG_TEMPERATURE_VALUE),
	(uint8_t*)(&app_regs.REG_ENABLE_TEMP_CALIBRATION),
	(uint8_t*)(&app_regs.REG_TEMP_USER_CALIBRATION),
	(uint8_t*)(&app_regs.REG_ENABLE_EVENTS)
};
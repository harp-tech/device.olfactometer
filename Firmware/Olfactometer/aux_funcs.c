#include "app_ios_and_regs.h"
#include "hwbp_core.h"
#include "app_funcs.h"
#include "aux_funcs.h"

extern status_PWM_DC_t status_DC;
extern countdown_t pulse_countdown;
extern uint8_t PWM_DC0_ready;
extern uint8_t PWM_DC1_ready;
extern uint8_t PWM_DC2_ready;
extern uint8_t PWM_DC3_ready;
extern uint8_t PWM_DC4_ready;
extern AppRegs app_regs;


/************************************************************************/
/* Get decimal divider from prescaler                                   */
/************************************************************************/
uint16_t get_divider(uint8_t prescaler)
{
	switch(prescaler)
	{
		case TIMER_PRESCALER_DIV1: return 1;
		case TIMER_PRESCALER_DIV2: return 2;
		case TIMER_PRESCALER_DIV4: return 4;
		case TIMER_PRESCALER_DIV8: return 8;
		case TIMER_PRESCALER_DIV64: return 64;
		case TIMER_PRESCALER_DIV256: return 256;
		case TIMER_PRESCALER_DIV1024: return 1024;
		default: return 0;
	}
}

uint8_t prescaler0, prescaler1, prescaler2, prescaler3, prescaler4;
uint16_t target_count0, target_count1, target_count2, target_count3 ,target_count4;
uint16_t duty_cycle0, duty_cycle1, duty_cycle2, duty_cycle3, duty_cycle4;

/************************************************************************/
/* Calculate valves PWM duty cycle value                                */
/************************************************************************/
void hwbp_app_pwm_gen_update_dc0(void)
{
	if (calculate_timer_16bits(32000000, app_regs.REG_CHANNEL0_FREQUENCY, &prescaler0, &target_count0))
	{
		duty_cycle0 = (((float)((uint16_t)(app_regs.REG_CHANNEL0_DUTY_CYCLE/100.0 * target_count0 + 0.5)) / target_count0)) * target_count0 + 0.5;
	}
}

void hwbp_app_pwm_gen_update_dc1(void)
{
	if (calculate_timer_16bits(32000000, app_regs.REG_CHANNEL1_FREQUENCY, &prescaler1, &target_count1))
	{
		duty_cycle1 = (((float)((uint16_t)(app_regs.REG_CHANNEL1_DUTY_CYCLE/100.0 * target_count1 + 0.5)) / target_count1)) * target_count1 + 0.5;
	}
}

void hwbp_app_pwm_gen_update_dc2(void)
{
	if (calculate_timer_16bits(32000000, app_regs.REG_CHANNEL2_FREQUENCY, &prescaler2, &target_count2))
	{
		duty_cycle2 = (((float)((uint16_t)(app_regs.REG_CHANNEL2_DUTY_CYCLE/100.0 * target_count2 + 0.5)) / target_count2)) * target_count2 + 0.5;
	}
}

void hwbp_app_pwm_gen_update_dc3(void)
{	
	if (calculate_timer_16bits(32000000, app_regs.REG_CHANNEL3_FREQUENCY, &prescaler3, &target_count3))
	{
		duty_cycle3 = (((float)((uint16_t)(app_regs.REG_CHANNEL3_DUTY_CYCLE/100.0 * target_count3 + 0.5)) / target_count3)) * target_count3 + 0.5;
	}
}

void hwbp_app_pwm_gen_update_dc4(void)
{
	if (calculate_timer_16bits(32000000, app_regs.REG_CHANNEL4_FREQUENCY, &prescaler4, &target_count4))
	{
		duty_cycle4 = (((float)((uint16_t)(app_regs.REG_CHANNEL4_DUTY_CYCLE/100.0 * target_count4 + 0.5)) / target_count4)) * target_count4 + 0.5;
	}
}


/************************************************************************/
/* Start valves PWM generation                                          */
/************************************************************************/

uint8_t hwbp_app_pwm_gen_start_ch0()
{
	if (!(TCC0_CTRLA))
	{
		timer_type0_pwm(&TCC0, prescaler0, target_count0, duty_cycle0, INT_LEVEL_LOW, INT_LEVEL_LOW);
		return 1;
	}   
    return 0;    
}


uint8_t hwbp_app_pwm_gen_start_ch1(void)
{
	if (!(TCD0_CTRLA))
    {
		timer_type0_pwm(&TCD0, prescaler1, target_count1, duty_cycle1, INT_LEVEL_LOW, INT_LEVEL_LOW);
		return 1;
    }            
    return 0;
}

uint8_t hwbp_app_pwm_gen_start_ch2(void)
{
	if (!(TCE0_CTRLA))
    {
		timer_type0_pwm(&TCE0, prescaler2, target_count2, duty_cycle2, INT_LEVEL_LOW, INT_LEVEL_LOW);
		return 1;
    }            
	return 0;
}

uint8_t hwbp_app_pwm_gen_start_ch3(void)
{
	if (!(TCF0_CTRLA))
	{
	    timer_type0_pwm(&TCF0, prescaler3, target_count3, duty_cycle3, INT_LEVEL_LOW, INT_LEVEL_LOW);
		return 1;
	}            
    return 0;
}

uint8_t hwbp_app_pwm_gen_start_ch4(void)
{
	// PWM mode for this timer was not implemented in core
	if (!(TCD1_CTRLA)){
		
		TC1_t* timer = &TCD1;
		uint8_t prescaler = prescaler4;
		uint16_t target_count = target_count4;
		uint16_t duty_cycle_count = duty_cycle4;
		uint8_t int_level_ovf = INT_LEVEL_LOW;
		uint8_t int_level_cca = INT_LEVEL_LOW;
		
		timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
		timer->CTRLFSET = TC_CMD_RESET_gc;		// Timer reset (registers to initial value)
		timer->PER = target_count-1;			// Set up target
		timer->CCA = duty_cycle_count;		    // Set duty cycle
		timer->INTCTRLA = int_level_ovf;		// Enable overflow interrupt
		timer->INTCTRLB = int_level_cca;		// Enable compare interrupt on channel A
		timer->CTRLB = TC1_CCAEN_bm | TC_WGMODE_SINGLESLOPE_gc; // Enable channel B and single slope mode
		timer->CTRLA = prescaler;				// Start timer
		return 1;
	}
    return 0;    
}


/************************************************************************/
/* Stop valves PWM generation                                           */
/************************************************************************/

uint8_t hwbp_app_pwm_gen_stop_ch0(void)
{
	if (TCC0_CTRLA)
    {
        timer_type0_stop(&TCC0);
		return 1;
	}
	
	return 0;
}


uint8_t hwbp_app_pwm_gen_stop_ch1(void)
{
	if (TCD0_CTRLA)
	{
    	timer_type0_stop(&TCD0);
		return 1;
    }
    
    return 0;
}

uint8_t hwbp_app_pwm_gen_stop_ch2(void)
{
	if (TCE0_CTRLA)
	{
    	timer_type0_stop(&TCE0);
		return 1;
    }
    
    return 0;
}

uint8_t hwbp_app_pwm_gen_stop_ch3(void)
{
	if (TCF0_CTRLA)
	{
    	timer_type0_stop(&TCF0);
		return 1;
    }
    
    return 0;
}

uint8_t hwbp_app_pwm_gen_stop_ch4(void)
{
	if (TCD1_CTRLA)
	{
    	timer_type1_stop(&TCD1);
		return 1;
    }
    
    return 0;
}

/************************************************************************/
/* PWM interrupts                                                       */
/************************************************************************/
ISR(TCC0_OVF_vect, ISR_NAKED)
{
	if(status_DC.DC0_ready){
		hwbp_app_pwm_gen_update_dc0();
		TC0_t* timer = &TCC0;
		timer->CCA = duty_cycle0;
		status_DC.DC0_ready = 0;
	}
	reti();
}

ISR(TCC0_CCA_vect, ISR_NAKED)
{
	reti();
}


ISR(TCD1_OVF_vect, ISR_NAKED)
{
	if(status_DC.DC4_ready){
		hwbp_app_pwm_gen_update_dc4();
		TC1_t* timer = &TCD1;
		timer->CCA = duty_cycle4;
		status_DC.DC4_ready = 0;
	}
	reti();
}

ISR(TCD1_CCA_vect, ISR_NAKED)
{
	reti();
}


ISR(TCD0_OVF_vect, ISR_NAKED)
{
	if(status_DC.DC1_ready){
		hwbp_app_pwm_gen_update_dc1();
		TC0_t* timer = &TCD0;
		timer->CCA = duty_cycle1;
		status_DC.DC1_ready = 0;
	}
	reti();
}

ISR(TCD0_CCA_vect, ISR_NAKED)
{
	reti();
}

ISR(TCE0_OVF_vect, ISR_NAKED)
{
	if(status_DC.DC2_ready){
		hwbp_app_pwm_gen_update_dc2();
		TC0_t* timer = &TCE0;
		timer->CCA = duty_cycle2;
		status_DC.DC2_ready = 0;
	}
	reti();
}

ISR(TCE0_CCA_vect, ISR_NAKED)
{
	reti();
}


ISR(TCF0_OVF_vect, ISR_NAKED)
{
	if(status_DC.DC3_ready){
		hwbp_app_pwm_gen_update_dc3();
		TC0_t* timer = &TCF0;
		timer->CCA = duty_cycle3;
		status_DC.DC3_ready = 0;
	}
	reti();
}

ISR(TCF0_CCA_vect, ISR_NAKED)
{
	reti();
}











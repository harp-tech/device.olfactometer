#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"
#include "aux_funcs.h"
#include "app.h"

extern countdown_t pulse_countdown;
extern status_PWM_DC_t status_DC;

/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_ENABLE_FLOW,
	&app_read_REG_FLOWMETER_ANALOG_OUTPUTS,
	&app_read_REG_DI0_STATE,
	&app_read_REG_CHANNEL0_USER_CALIBRATION,
	&app_read_REG_CHANNEL1_USER_CALIBRATION,
	&app_read_REG_CHANNEL2_USER_CALIBRATION,
	&app_read_REG_CHANNEL3_USER_CALIBRATION,
	&app_read_REG_CHANNEL4_USER_CALIBRATION,
	&app_read_REG_CHANNEL3_USER_CALIBRATION_AUX,
	&app_read_REG_USER_CALIBRATION_ENABLE,
	&app_read_REG_CHANNEL0_FLOW_TARGET,
	&app_read_REG_CHANNEL1_FLOW_TARGET,
	&app_read_REG_CHANNEL2_FLOW_TARGET,
	&app_read_REG_CHANNEL3_FLOW_TARGET,
	&app_read_REG_CHANNEL4_FLOW_TARGET,
	&app_read_REG_CHANNEL0_FLOW_REAL,
	&app_read_REG_CHANNEL1_FLOW_REAL,
	&app_read_REG_CHANNEL2_FLOW_REAL,
	&app_read_REG_CHANNEL3_FLOW_REAL,
	&app_read_REG_CHANNEL4_FLOW_REAL,
	&app_read_REG_CHANNEL0_FREQUENCY,
	&app_read_REG_CHANNEL1_FREQUENCY,
	&app_read_REG_CHANNEL2_FREQUENCY,
	&app_read_REG_CHANNEL3_FREQUENCY,
	&app_read_REG_CHANNEL4_FREQUENCY,
	&app_read_REG_CHANNEL0_DUTY_CYCLE,
	&app_read_REG_CHANNEL1_DUTY_CYCLE,
	&app_read_REG_CHANNEL2_DUTY_CYCLE,
	&app_read_REG_CHANNEL3_DUTY_CYCLE,
	&app_read_REG_CHANNEL4_DUTY_CYCLE,
	&app_read_REG_OUTPUT_SET,
	&app_read_REG_OUTPUT_CLEAR,
	&app_read_REG_OUTPUT_TOGGLE,
	&app_read_REG_OUTPUT_STATE,
	&app_read_REG_ENABLE_VALVES_PULSE,
	&app_read_REG_VALVES_SET,
	&app_read_REG_VALVES_CLEAR,
	&app_read_REG_VALVES_TOGGLE,
	&app_read_REG_VALVES_STATE,
	&app_read_REG_PULSE_VALVE0,
	&app_read_REG_PULSE_VALVE1,
	&app_read_REG_PULSE_VALVE2,
	&app_read_REG_PULSE_VALVE3,
	&app_read_REG_PULSE_ENDVALVE0,
	&app_read_REG_PULSE_ENDVALVE1,
	&app_read_REG_PULSE_DUMMYVALVE,
	&app_read_REG_DO0_SYNC,
	&app_read_REG_DO1_SYNC,
	&app_read_REG_DI0_TRIGGER,
	&app_read_REG_MIMIC_VALVE0,
	&app_read_REG_MIMIC_VALVE1,
	&app_read_REG_MIMIC_VALVE2,
	&app_read_REG_MIMIC_VALVE3,
	&app_read_REG_MIMIC_ENDVALVE0,
	&app_read_REG_MIMIC_ENDVALVE1,
	&app_read_REG_MIMIC_DUMMYVALVE,
	&app_read_REG_EXT_CTRL_VALVES,
	&app_read_REG_CHANNEL3_RANGE,
	&app_read_REG_RESERVED0,
	&app_read_REG_RESERVED1,
	&app_read_REG_RESERVED2,
	&app_read_REG_ENABLE_EVENTS
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_ENABLE_FLOW,
	&app_write_REG_FLOWMETER_ANALOG_OUTPUTS,
	&app_write_REG_DI0_STATE,
	&app_write_REG_CHANNEL0_USER_CALIBRATION,
	&app_write_REG_CHANNEL1_USER_CALIBRATION,
	&app_write_REG_CHANNEL2_USER_CALIBRATION,
	&app_write_REG_CHANNEL3_USER_CALIBRATION,
	&app_write_REG_CHANNEL4_USER_CALIBRATION,
	&app_write_REG_CHANNEL3_USER_CALIBRATION_AUX,
	&app_write_REG_USER_CALIBRATION_ENABLE,
	&app_write_REG_CHANNEL0_FLOW_TARGET,
	&app_write_REG_CHANNEL1_FLOW_TARGET,
	&app_write_REG_CHANNEL2_FLOW_TARGET,
	&app_write_REG_CHANNEL3_FLOW_TARGET,
	&app_write_REG_CHANNEL4_FLOW_TARGET,
	&app_write_REG_CHANNEL0_FLOW_REAL,
	&app_write_REG_CHANNEL1_FLOW_REAL,
	&app_write_REG_CHANNEL2_FLOW_REAL,
	&app_write_REG_CHANNEL3_FLOW_REAL,
	&app_write_REG_CHANNEL4_FLOW_REAL,
	&app_write_REG_CHANNEL0_FREQUENCY,
	&app_write_REG_CHANNEL1_FREQUENCY,
	&app_write_REG_CHANNEL2_FREQUENCY,
	&app_write_REG_CHANNEL3_FREQUENCY,
	&app_write_REG_CHANNEL4_FREQUENCY,
	&app_write_REG_CHANNEL0_DUTY_CYCLE,
	&app_write_REG_CHANNEL1_DUTY_CYCLE,
	&app_write_REG_CHANNEL2_DUTY_CYCLE,
	&app_write_REG_CHANNEL3_DUTY_CYCLE,
	&app_write_REG_CHANNEL4_DUTY_CYCLE,
	&app_write_REG_OUTPUT_SET,
	&app_write_REG_OUTPUT_CLEAR,
	&app_write_REG_OUTPUT_TOGGLE,
	&app_write_REG_OUTPUT_STATE,
	&app_write_REG_ENABLE_VALVES_PULSE,
	&app_write_REG_VALVES_SET,
	&app_write_REG_VALVES_CLEAR,
	&app_write_REG_VALVES_TOGGLE,
	&app_write_REG_VALVES_STATE,
	&app_write_REG_PULSE_VALVE0,
	&app_write_REG_PULSE_VALVE1,
	&app_write_REG_PULSE_VALVE2,
	&app_write_REG_PULSE_VALVE3,
	&app_write_REG_PULSE_ENDVALVE0,
	&app_write_REG_PULSE_ENDVALVE1,
	&app_write_REG_PULSE_DUMMYVALVE,
	&app_write_REG_DO0_SYNC,
	&app_write_REG_DO1_SYNC,
	&app_write_REG_DI0_TRIGGER,
	&app_write_REG_MIMIC_VALVE0,
	&app_write_REG_MIMIC_VALVE1,
	&app_write_REG_MIMIC_VALVE2,
	&app_write_REG_MIMIC_VALVE3,
	&app_write_REG_MIMIC_ENDVALVE0,
	&app_write_REG_MIMIC_ENDVALVE1,
	&app_write_REG_MIMIC_DUMMYVALVE,
	&app_write_REG_EXT_CTRL_VALVES,
	&app_write_REG_CHANNEL3_RANGE,
	&app_write_REG_RESERVED0,
	&app_write_REG_RESERVED1,
	&app_write_REG_RESERVED2,
	&app_write_REG_ENABLE_EVENTS
};


/************************************************************************/
/* Stop and update DC of valves PWM                                     */
/************************************************************************/

void stop_and_update_ch0_temps(void)
{
	hwbp_app_pwm_gen_stop_ch0();
	hwbp_app_pwm_gen_update_dc0();
}

void stop_and_update_ch1_temps(void)
{       
	hwbp_app_pwm_gen_stop_ch1();
    hwbp_app_pwm_gen_update_dc1();
}

void stop_and_update_ch2_temps(void)
{       
	hwbp_app_pwm_gen_stop_ch2();
    hwbp_app_pwm_gen_update_dc2();
}

void stop_and_update_ch3_temps(void)
{       
	hwbp_app_pwm_gen_stop_ch3();
    hwbp_app_pwm_gen_update_dc3();
}

void stop_and_update_ch4_temps(void)
{       
	hwbp_app_pwm_gen_stop_ch4();
    hwbp_app_pwm_gen_update_dc4();
}


/************************************************************************/
/* REG_ENABLE_FLOW                                                      */
/************************************************************************/
void app_read_REG_ENABLE_FLOW(void)
{
	//app_regs.REG_ENABLE_FLOW = 0;

}

bool app_write_REG_ENABLE_FLOW(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	float low_limit_dc = 1;

	if((app_regs.REG_DO1_SYNC & MSK_DOUT1_CONF) == GM_DOUT1_START){
		if (reg & B_START) set_OUT1; else clr_OUT1;}
	
	if((app_regs.REG_DO0_SYNC & MSK_DOUT0_CONF) == GM_DOUT0_START){
		if (reg & B_START) set_OUT0; else clr_OUT0;}
	
	
	if (!(reg & B_START)){
		hwbp_app_pwm_gen_stop_ch0();
		hwbp_app_pwm_gen_stop_ch1();
		hwbp_app_pwm_gen_stop_ch2();
		hwbp_app_pwm_gen_stop_ch3();
		hwbp_app_pwm_gen_stop_ch4();
		
		app_write_REG_CHANNEL0_DUTY_CYCLE(&low_limit_dc);
		app_write_REG_CHANNEL1_DUTY_CYCLE(&low_limit_dc);
		app_write_REG_CHANNEL2_DUTY_CYCLE(&low_limit_dc);
		app_write_REG_CHANNEL3_DUTY_CYCLE(&low_limit_dc);
		app_write_REG_CHANNEL4_DUTY_CYCLE(&low_limit_dc);
				
		clr_ENDVALVE0;
		clr_ENDVALVE1;
		clr_VALVE0;
		clr_VALVE1;
		clr_VALVE2;
		clr_VALVE3;
		
	}
	
	app_regs.REG_ENABLE_FLOW = reg;
	
	return true;
}


/************************************************************************/
/* REG_FLOWMETER_ANALOG_OUTPUTS                                         */
/************************************************************************/
// This register is an array with 5 positions
void app_read_REG_FLOWMETER_ANALOG_OUTPUTS(void)
{
	//app_regs.FLOWMETER_ANALOG_OUTPUTS[0] = 0;

}

bool app_write_REG_FLOWMETER_ANALOG_OUTPUTS(void *a)
{
	int16_t *reg = ((int16_t*)a);

	app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DI0_STATE                                                              */
/************************************************************************/
void app_read_REG_DI0_STATE(void)
{
	//app_regs.REG_DI0_STATE = 0;

}

bool app_write_REG_DI0_STATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI0_STATE = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL0_USER_CALIBRATION                                        */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL0_USER_CALIBRATION(void)
{
	//app_regs.REG_CHANNEL0_USER_CALIBRATION[0] = 0;

}

bool app_write_REG_CHANNEL0_USER_CALIBRATION(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL0_USER_CALIBRATION[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CHANNEL1_USER_CALIBRATION                                        */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL1_USER_CALIBRATION(void)
{
	//app_regs.REG_CHANNEL1_USER_CALIBRATION[0] = 0;

}

bool app_write_REG_CHANNEL1_USER_CALIBRATION(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL1_USER_CALIBRATION[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CHANNEL2_USER_CALIBRATION                                        */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL2_USER_CALIBRATION(void)
{
	//app_regs.REG_CHANNEL2_USER_CALIBRATION[0] = 0;

}

bool app_write_REG_CHANNEL2_USER_CALIBRATION(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL2_USER_CALIBRATION[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_USER_CALIBRATION                                        */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL3_USER_CALIBRATION(void)
{
	//app_regs.REG_CHANNEL3_USER_CALIBRATION[0] = 0;

}

bool app_write_REG_CHANNEL3_USER_CALIBRATION(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL3_USER_CALIBRATION[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CHANNEL4_USER_CALIBRATION                                        */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL4_USER_CALIBRATION(void)
{
	//app_regs.REG_CHANNEL4_USER_CALIBRATION[0] = 0;

}

bool app_write_REG_CHANNEL4_USER_CALIBRATION(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL4_USER_CALIBRATION[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_USER_CALIBRATION_AUX                                    */
/************************************************************************/
// This register is an array with 11 positions
void app_read_REG_CHANNEL3_USER_CALIBRATION_AUX(void)
{
	//app_regs.REG_CHANNEL3_USER_CALIBRATION_AUX[0] = 0;

}

bool app_write_REG_CHANNEL3_USER_CALIBRATION_AUX(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CHANNEL3_USER_CALIBRATION_AUX[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_USER_CALIBRATION_ENABLE                                          */
/************************************************************************/
void app_read_REG_USER_CALIBRATION_ENABLE(void)
{
	//app_regs.REG_USER_CALIBRATION_ENABLE = 0;

}

bool app_write_REG_USER_CALIBRATION_ENABLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_USER_CALIBRATION_ENABLE = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL0_FLOW_TARGET                                             */
/************************************************************************/
void app_read_REG_CHANNEL0_FLOW_TARGET(void)
{
	//app_regs.REG_CHANNEL0_FLOW_TARGET = 0;

}

bool app_write_REG_CHANNEL0_FLOW_TARGET(void *a)
{
	float reg = *((float*)a);
	
	if (reg > 110)
		app_regs.REG_CHANNEL0_FLOW_TARGET = 110;
	else
		app_regs.REG_CHANNEL0_FLOW_TARGET = reg;
	
	return true;
}


/************************************************************************/
/* REG_CHANNEL1_FLOW_TARGET                                             */
/************************************************************************/
void app_read_REG_CHANNEL1_FLOW_TARGET(void)
{
	//app_regs.REG_CHANNEL1_FLOW_TARGET = 0;

}

bool app_write_REG_CHANNEL1_FLOW_TARGET(void *a)
{
	float reg = *((float*)a);
	
	if (reg > 110)
		app_regs.REG_CHANNEL1_FLOW_TARGET = 110;
	else
		app_regs.REG_CHANNEL1_FLOW_TARGET = reg;

	return true;
}


/************************************************************************/
/* REG_CHANNEL2_FLOW_TARGET                                             */
/************************************************************************/
void app_read_REG_CHANNEL2_FLOW_TARGET(void)
{
	//app_regs.REG_CHANNEL2_FLOW_TARGET = 0;

}

bool app_write_REG_CHANNEL2_FLOW_TARGET(void *a)
{
	float reg = *((float*)a);

	if (reg > 110)
		app_regs.REG_CHANNEL2_FLOW_TARGET = 110;
	else
		app_regs.REG_CHANNEL2_FLOW_TARGET = reg;
		
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_FLOW_TARGET                                                  */
/************************************************************************/
void app_read_REG_CHANNEL3_FLOW_TARGET(void)
{
	//app_regs.REG_CHANNEL3_FLOW_TARGET = 0;

}

bool app_write_REG_CHANNEL3_FLOW_TARGET(void *a)
{
	float reg = *((float*)a);
	
	if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
		if (reg > 110)
			app_regs.REG_CHANNEL3_FLOW_TARGET = 110;
		else
			app_regs.REG_CHANNEL3_FLOW_TARGET = reg;
	}
	else{
		if (reg > 1100)
			app_regs.REG_CHANNEL3_FLOW_TARGET = 1100;
		else
			app_regs.REG_CHANNEL3_FLOW_TARGET = reg;
	}
		
	return true;
}


/************************************************************************/
/* REG_CHANNEL4_FLOW_TARGET                                                  */
/************************************************************************/
void app_read_REG_CHANNEL4_FLOW_TARGET(void)
{
	//app_regs.REG_CH4_FLOW_TARGET = 0;

}

bool app_write_REG_CHANNEL4_FLOW_TARGET(void *a)
{
	float reg = *((float*)a);

	if (reg > 1100)
		app_regs.REG_CHANNEL4_FLOW_TARGET = 1100;
	else
		app_regs.REG_CHANNEL4_FLOW_TARGET = reg;
		
	return true;
}


/************************************************************************/
/* REG_CHANNEL0_FLOW_REAL                                               */
/************************************************************************/
void app_read_REG_CHANNEL0_FLOW_REAL(void)
{
	//app_regs.REG_CHANNEL0_FLOW_REAL = 0;

}

bool app_write_REG_CHANNEL0_FLOW_REAL(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_CHANNEL0_FLOW_REAL = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL1_FLOW_REAL                                               */
/************************************************************************/
void app_read_REG_CHANNEL1_FLOW_REAL(void)
{
	//app_regs.REG_CHANNEL1_FLOW_REAL = 0;

}

bool app_write_REG_CHANNEL1_FLOW_REAL(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_CHANNEL1_FLOW_REAL = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL2_FLOW_REAL                                               */
/************************************************************************/
void app_read_REG_CHANNEL2_FLOW_REAL(void)
{
	//app_regs.REG_CHANNEL2_FLOW_REAL = 0;

}

bool app_write_REG_CHANNEL2_FLOW_REAL(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_CHANNEL2_FLOW_REAL = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_FLOW_REAL                                                    */
/************************************************************************/
void app_read_REG_CHANNEL3_FLOW_REAL(void)
{
	//app_regs.REG_CHANNEL3_FLOW_REAL = 0;

}

bool app_write_REG_CHANNEL3_FLOW_REAL(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_CHANNEL3_FLOW_REAL = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL4_FLOW_REAL                                               */
/************************************************************************/
void app_read_REG_CHANNEL4_FLOW_REAL(void)
{
	//app_regs.REG_CHANNEL4_FLOW_REAL = 0;

}

bool app_write_REG_CHANNEL4_FLOW_REAL(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_CHANNEL4_FLOW_REAL = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL0_FREQUENCY                                               */
/************************************************************************/
void app_read_REG_CHANNEL0_FREQUENCY(void)
{
	//app_regs.REG_CHANNEL0_FREQUENCY = 0;

}

bool app_write_REG_CHANNEL0_FREQUENCY(void *a)
{
	
	uint16_t reg = *((uint16_t*)a);
	//float reg = *((float*)a);
	
	if (reg < 100 || reg > 10000)
		return false;

	app_regs.REG_CHANNEL0_FREQUENCY = reg;
	stop_and_update_ch0_temps();
	return true;
}


/************************************************************************/
/* REG_CHANNEL1_FREQUENCY                                               */
/************************************************************************/
void app_read_REG_CHANNEL1_FREQUENCY(void)
{
	//app_regs.REG_CHANNEL1_FREQUENCY = 0;

}

bool app_write_REG_CHANNEL1_FREQUENCY(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	//float reg = *((float*)a);
	
	if (reg < 100 || reg > 10000)
		return false;

	app_regs.REG_CHANNEL1_FREQUENCY = reg;
	stop_and_update_ch1_temps();
	return true;
}


/************************************************************************/
/* REG_CHANNEL2_FREQUENCY                                               */
/************************************************************************/
void app_read_REG_CHANNEL2_FREQUENCY(void)
{
	//app_regs.REG_CHANNEL2_FREQUENCY = 0;

}

bool app_write_REG_CHANNEL2_FREQUENCY(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	//float reg = *((float*)a);
	
	if (reg < 100 || reg > 10000)
		return false;

	app_regs.REG_CHANNEL2_FREQUENCY = reg;
	stop_and_update_ch2_temps();
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_FREQUENCY                                               */
/************************************************************************/
void app_read_REG_CHANNEL3_FREQUENCY(void)
{
	//app_regs.REG_CHANNEL3_FREQUENCY = 0;

}

bool app_write_REG_CHANNEL3_FREQUENCY(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	//float reg = *((float*)a);
	
	if (reg < 100 || reg > 10000)
		return false;

	app_regs.REG_CHANNEL3_FREQUENCY = reg;
	stop_and_update_ch3_temps();
	return true;
}


/************************************************************************/
/* REG_CHANNEL4_FREQUENCY                                               */
/************************************************************************/
void app_read_REG_CHANNEL4_FREQUENCY(void)
{
	//app_regs.REG_CHANNEL4_FREQ = 0;

}

bool app_write_REG_CHANNEL4_FREQUENCY(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	//float reg = *((float*)a);
	
	if (reg < 100 || reg > 10000)
		return false;

	app_regs.REG_CHANNEL4_FREQUENCY = reg;
	stop_and_update_ch4_temps();
	return true;
}

/************************************************************************/
/* REG_CHANNEL0_DUTY_CYCLE                                              */
/************************************************************************/
void app_read_REG_CHANNEL0_DUTY_CYCLE(void)
{
	//app_regs.REG_CHANNEL0_DUTY_CYCLE = 0;

}

bool app_write_REG_CHANNEL0_DUTY_CYCLE(void *a)
{
	float reg = *((float*)a);
	status_DC.DC0_ready = 1;
		
	if (reg <= 0.1 || reg >= 99.9)
		return false;

	app_regs.REG_CHANNEL0_DUTY_CYCLE = reg;
	
	if (app_regs.REG_ENABLE_FLOW & B_START){
		if (!TCC0_CTRLA){
			hwbp_app_pwm_gen_start_ch0();
		}
	}
	
	return true;
}


/************************************************************************/
/* REG_CHANNEL1_DUTY_CYCLE                                              */
/************************************************************************/
void app_read_REG_CHANNEL1_DUTY_CYCLE(void)
{
	//app_regs.REG_CHANNEL1_DUTY_CYCLE = 0;

}

bool app_write_REG_CHANNEL1_DUTY_CYCLE(void *a)
{
	float reg = *((float*)a);
	status_DC.DC1_ready = 1;
	
	if (reg <= 0.1 || reg >= 99.9)
		return false;

	app_regs.REG_CHANNEL1_DUTY_CYCLE = reg;
	
	if (app_regs.REG_ENABLE_FLOW & B_START){
		if (!TCD0_CTRLA){
			hwbp_app_pwm_gen_start_ch1();
		}
	}
	
	return true;
}


/************************************************************************/
/* REG_CHANNEL2_DUTY_CYCLE                                              */
/************************************************************************/
void app_read_REG_CHANNEL2_DUTY_CYCLE(void)
{
	//app_regs.REG_CH2_DUTYCYCLE = 0;

}

bool app_write_REG_CHANNEL2_DUTY_CYCLE(void *a)
{
	float reg = *((float*)a);
	status_DC.DC2_ready = 1;
	
	if (reg <= 0.1 || reg >= 99.9)
		return false;

	app_regs.REG_CHANNEL2_DUTY_CYCLE = reg;
	
	if (app_regs.REG_ENABLE_FLOW & B_START){
		if (!TCE0_CTRLA){
			hwbp_app_pwm_gen_start_ch2();
		}
	}

	return true;
}


/************************************************************************/
/* REG_CHANNEL3_DUTY_CYCLE                                              */
/************************************************************************/
void app_read_REG_CHANNEL3_DUTY_CYCLE(void)
{
	//app_regs.REG_CHANNEL3_DUTY_CYCLE = 0;

}

bool app_write_REG_CHANNEL3_DUTY_CYCLE(void *a)
{
	float reg = *((float*)a);
	status_DC.DC3_ready = 1;

	if (reg <= 0.1 || reg >= 99.9)
		return false;

	app_regs.REG_CHANNEL3_DUTY_CYCLE = reg;
	
	if (app_regs.REG_ENABLE_FLOW & B_START){
		if (!TCF0_CTRLA){
			hwbp_app_pwm_gen_start_ch3();
		}
	}

	return true;
}


/************************************************************************/
/* REG_CHANNEL4_DUTY_CYCLE                                              */
/************************************************************************/
void app_read_REG_CHANNEL4_DUTY_CYCLE(void)
{
	//app_regs.REG_CHANNEL4_DUTYCYCLE = 0;

}

bool app_write_REG_CHANNEL4_DUTY_CYCLE(void *a)
{
	float reg = *((float*)a);
	status_DC.DC4_ready = 1; 

	if (reg <= 0.1 || reg >= 99.9)
		return false;

	app_regs.REG_CHANNEL4_DUTY_CYCLE = reg;
	
	if (app_regs.REG_ENABLE_FLOW & B_START){
		if (!TCD1_CTRLA){
			hwbp_app_pwm_gen_start_ch4();
		}
	}

	return true;
}


/************************************************************************/
/* REG_OUTPUT_SET                                                      */
/************************************************************************/
void app_read_REG_OUTPUT_SET(void)
{
	//app_regs.REG_OUTPUT_SET = 0;

}

bool app_write_REG_OUTPUT_SET(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	//app_regs.REG_OUTPUT_SET = reg;
	//return true;
	
	if((app_regs.REG_DO0_SYNC & MSK_DOUT0_CONF) == GM_DOUT0_SOFTWARE)
		if (reg & B_DOUT0) set_OUT0;
	
	if((app_regs.REG_DO1_SYNC & MSK_DOUT1_CONF) == GM_DOUT1_SOFTWARE)
		if (reg & B_DOUT1) set_OUT1;

	app_regs.REG_OUTPUT_STATE |= reg;
	app_regs.REG_OUTPUT_SET = reg;
	
	return true;
}


/************************************************************************/
/* REG_OUTPUT_CLEAR                                                     */
/************************************************************************/
void app_read_REG_OUTPUT_CLEAR(void)
{
	//app_regs.REG_OUTPUT_CLEAR = 0;

}

bool app_write_REG_OUTPUT_CLEAR(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if((app_regs.REG_DO0_SYNC & MSK_DOUT0_CONF) == GM_DOUT0_SOFTWARE)
		if (reg & B_DOUT0) clr_OUT0;
	
	if((app_regs.REG_DO0_SYNC & MSK_DOUT1_CONF) == GM_DOUT1_SOFTWARE)
		if (reg & B_DOUT1) clr_OUT1;
	
	app_regs.REG_OUTPUT_STATE &= ~reg;
	app_regs.REG_OUTPUT_CLEAR = reg;

	return true;
}


/************************************************************************/
/* REG_OUTPUT_TOGGLE                                                    */
/************************************************************************/
void app_read_REG_OUTPUT_TOGGLE(void)
{
	//app_regs.REG_OUTPUT_TOGGLE = 0;

}

bool app_write_REG_OUTPUT_TOGGLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if((app_regs.REG_DO0_SYNC & MSK_DOUT0_CONF) == GM_DOUT0_SOFTWARE)
		if (reg & B_DOUT0) { if (read_OUT0) tgl_OUT0; else set_OUT0;}
		
	if((app_regs.REG_DO0_SYNC & MSK_DOUT1_CONF) == GM_DOUT1_SOFTWARE)
		if (reg & B_DOUT1) { if (read_OUT1) tgl_OUT1; else set_OUT1;}

	app_regs.REG_OUTPUT_STATE ^= reg;
	app_regs.REG_OUTPUT_TOGGLE = reg;

	return true;
}


/************************************************************************/
/* REG_OUTPUT_STATE                                                       */
/************************************************************************/
void app_read_REG_OUTPUT_STATE(void)
{
	//app_regs.REG_OUTPUT_OUT = 0;
	app_regs.REG_OUTPUT_STATE |= (read_OUT0) ? B_DOUT0 : 0;
	app_regs.REG_OUTPUT_STATE |= (read_OUT1) ? B_DOUT1 : 0;

}

bool app_write_REG_OUTPUT_STATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if((app_regs.REG_DO0_SYNC & MSK_DOUT0_CONF) == GM_DOUT0_SOFTWARE){
		if (reg & B_DOUT0) set_OUT0; else clr_OUT0;}
	
	if((app_regs.REG_DO0_SYNC & MSK_DOUT1_CONF) == GM_DOUT1_SOFTWARE){
		if (reg & B_DOUT1) set_OUT1; else clr_OUT1;}
	
	app_regs.REG_OUTPUT_STATE = reg;
	return true;
}


/************************************************************************/
/* REG_ENABLE_VALVES_PULSE                                                  */
/************************************************************************/
void app_read_REG_ENABLE_VALVES_PULSE(void)
{
	//app_regs.REG_ENABLE_VALVES_PULSE = 0;

}

bool app_write_REG_ENABLE_VALVES_PULSE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_ENABLE_VALVES_PULSE = reg;
	return true;
}


/************************************************************************/
/* REG_VALVES_SET                                                       */
/************************************************************************/

/*****************************************************************************************************************/

#define start_VALVE0 do {set_VALVE0; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE0) pulse_countdown.valve0 = app_regs.REG_PULSE_VALVE0 + 1; } while(0)
#define start_VALVE1 do {set_VALVE1; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE1) pulse_countdown.valve1 = app_regs.REG_PULSE_VALVE1 + 1; } while(0)
#define start_VALVE2 do {set_VALVE2; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE2) pulse_countdown.valve2 = app_regs.REG_PULSE_VALVE2 + 1; } while(0)
#define start_VALVE3 do {set_VALVE3; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE3) pulse_countdown.valve3 = app_regs.REG_PULSE_VALVE3 + 1; } while(0)
#define start_VALVEAUX0 do {set_ENDVALVE0; if (app_regs.REG_ENABLE_VALVES_PULSE & B_ENDVALVE0) pulse_countdown.valveaux0 = app_regs.REG_PULSE_ENDVALVE0 + 1; } while(0)
#define start_VALVEAUX1 do {set_ENDVALVE1; if (app_regs.REG_ENABLE_VALVES_PULSE & B_ENDVALVE1) pulse_countdown.valveaux1 = app_regs.REG_PULSE_ENDVALVE1 + 1; } while(0)
#define start_VALVEDUMMY do {set_DUMMYVALVE; if (app_regs.REG_ENABLE_VALVES_PULSE & B_DUMMYVALVE) pulse_countdown.valvedummy = app_regs.REG_PULSE_DUMMYVALVE + 1; } while(0)
	
void app_read_REG_VALVES_SET(void)
{
	//app_regs.REG_VALVES_SET = 0;

}

bool app_write_REG_VALVES_SET(void *a)
{
	uint8_t reg = *((uint8_t*)a);
				
	if (reg & B_VALVE0) start_VALVE0;
	if (reg & B_VALVE1) start_VALVE1;
	if (reg & B_VALVE2) start_VALVE2;
	if (reg & B_VALVE3) start_VALVE3;
	if (reg & B_ENDVALVE0) start_VALVEAUX0;
	if (reg & B_ENDVALVE1) start_VALVEAUX1;
	if (reg & B_DUMMYVALVE) start_VALVEDUMMY;
		
	app_regs.REG_VALVES_STATE |= reg;
	app_regs.REG_VALVES_SET = reg;

	return true;
}


/************************************************************************/
/* REG_VALVES_CLEAR                                                     */
/************************************************************************/

void app_read_REG_VALVES_CLEAR(void)
{
	//app_regs.REG_VALVES_CLEAR = 0;

}

bool app_write_REG_VALVES_CLEAR(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if (reg & B_VALVE0) clr_VALVE0;
	if (reg & B_VALVE1) clr_VALVE1;
	if (reg & B_VALVE2) clr_VALVE2;
	if (reg & B_VALVE3) clr_VALVE3;
	if (reg & B_ENDVALVE0) clr_ENDVALVE0;
	if (reg & B_ENDVALVE1) clr_ENDVALVE1;
	if (reg & B_DUMMYVALVE) clr_DUMMYVALVE;
	
	app_regs.REG_VALVES_STATE &= ~reg;
	app_regs.REG_VALVES_CLEAR = reg;

	return true;
}


/************************************************************************/
/* REG_VALVES_TOGGLE                                                    */
/************************************************************************/


void app_read_REG_VALVES_TOGGLE(void)
{
	//app_regs.REG_VALVES_TOGGLE = 0;

}

bool app_write_REG_VALVES_TOGGLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_VALVE0) { if (read_VALVE0) tgl_VALVE0; else start_VALVE0;}
	if (reg & B_VALVE1) { if (read_VALVE1) tgl_VALVE1; else start_VALVE1;}
	if (reg & B_VALVE2) { if (read_VALVE2) tgl_VALVE2; else start_VALVE2;}
	if (reg & B_VALVE3) { if (read_VALVE3) tgl_VALVE3; else start_VALVE3;}
	if (reg & B_ENDVALVE0) { if (read_ENDVALVE0) tgl_ENDVALVE0; else start_VALVEAUX0;}
	if (reg & B_ENDVALVE1) { if (read_ENDVALVE1) tgl_ENDVALVE1; else start_VALVEAUX1;}
	if (reg & B_DUMMYVALVE) { if (read_DUMMYVALVE) tgl_DUMMYVALVE; else start_VALVEDUMMY;}
	
	app_regs.REG_VALVES_STATE ^= reg;
	app_regs.REG_VALVES_TOGGLE = reg;

	return true;
}


/************************************************************************/
/* REG_VALVES_STATE                                                     */
/************************************************************************/

void app_read_REG_VALVES_STATE(void)
{
	//app_regs.REG_VALVES_STATE = 0;
	
	app_regs.REG_VALVES_STATE |= (read_VALVE0) ? B_VALVE0 : 0;
	app_regs.REG_VALVES_STATE |= (read_VALVE1) ? B_VALVE1 : 0;
	app_regs.REG_VALVES_STATE |= (read_VALVE2) ? B_VALVE2 : 0;
	app_regs.REG_VALVES_STATE |= (read_VALVE3) ? B_VALVE3 : 0;
	app_regs.REG_VALVES_STATE |= (read_ENDVALVE0) ? B_ENDVALVE0 : 0;
	app_regs.REG_VALVES_STATE |= (read_ENDVALVE1) ? B_ENDVALVE1 : 0;
	app_regs.REG_VALVES_STATE |= (read_DUMMYVALVE) ? B_DUMMYVALVE : 0;

}

bool app_write_REG_VALVES_STATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if (reg & B_VALVE0) start_VALVE0; else clr_VALVE0;
	if (reg & B_VALVE1) start_VALVE1; else clr_VALVE1;
	if (reg & B_VALVE2) start_VALVE2; else clr_VALVE2;
	if (reg & B_VALVE3) start_VALVE3; else clr_VALVE3;
	if (reg & B_ENDVALVE0) start_VALVEAUX0; else clr_ENDVALVE0;
	if (reg & B_ENDVALVE1) start_VALVEAUX1; else clr_ENDVALVE1;
	if (reg & B_DUMMYVALVE) start_VALVEDUMMY; else clr_DUMMYVALVE;

	app_regs.REG_VALVES_STATE = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_VALVE0                                                     */
/************************************************************************/
void app_read_REG_PULSE_VALVE0(void) 
{
	
	//app_regs.REG_PULSE_VALVE0 = 0;
}

bool app_write_REG_PULSE_VALVE0(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	if (reg < 1)
		return false;

	app_regs.REG_PULSE_VALVE0 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_VALVE1                                                     */
/************************************************************************/
void app_read_REG_PULSE_VALVE1(void)
{

	//app_regs.REG_PULSE_VALVE1 = 0;
}
bool app_write_REG_PULSE_VALVE1(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	if (reg < 1)
		return false;

	app_regs.REG_PULSE_VALVE1 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_VALVE2                                                     */
/************************************************************************/
void app_read_REG_PULSE_VALVE2(void) 
{

	//app_regs.REG_PULSE_VALVE2 = 0;
}

bool app_write_REG_PULSE_VALVE2(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	if (reg < 1)
		return false;

	app_regs.REG_PULSE_VALVE2 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_VALVE3                                                     */
/************************************************************************/
void app_read_REG_PULSE_VALVE3(void)
{

//app_regs.REG_PULSE_VALVE3 = 0;
}
	
	
bool app_write_REG_PULSE_VALVE3(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	if (reg < 1)
		return false;

	app_regs.REG_PULSE_VALVE3 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_ENDVALVE0                                                  */
/************************************************************************/
void app_read_REG_PULSE_ENDVALVE0(void)
{
	//app_regs.REG_PULSE_VALVE_AUX0 = 0;

}

bool app_write_REG_PULSE_ENDVALVE0(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	if (reg < 1)
		return false;
	
	app_regs.REG_PULSE_ENDVALVE0 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_ENDVALVE1                                                  */
/************************************************************************/
void app_read_REG_PULSE_ENDVALVE1(void)
{
	//app_regs.REG_PULSE_ENDVALVE1 = 0;

}

bool app_write_REG_PULSE_ENDVALVE1(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	if (reg < 1)
		return false;
	
	app_regs.REG_PULSE_ENDVALVE1 = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_DUMMYVALVE                                                 */
/************************************************************************/
void app_read_REG_PULSE_DUMMYVALVE(void)
{
	//app_regs.REG_PULSE_DUMMYVALVE = 0;

}

bool app_write_REG_PULSE_DUMMYVALVE(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg < 1)
		return false;
	
	app_regs.REG_PULSE_DUMMYVALVE = reg;
	return true;
}


/************************************************************************/
/* REG_DO0_SYNC                                                         */
/************************************************************************/
void app_read_REG_DO0_SYNC(void)
{
	//app_regs.REG_DO0_SYNC = 0;

}

bool app_write_REG_DO0_SYNC(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO0_SYNC = reg;
	return true;
}


/************************************************************************/
/* REG_DO1_SYNC                                                         */
/************************************************************************/
void app_read_REG_DO1_SYNC(void)
{
	//app_regs.REG_DO1_SYNC = 0;

}

bool app_write_REG_DO1_SYNC(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO1_SYNC = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_TRIGGER                                                      */
/************************************************************************/
void app_read_REG_DI0_TRIGGER(void)
{
	//app_regs.REG_DI0_TRIGGER = 0;

}

bool app_write_REG_DI0_TRIGGER(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI0_TRIGGER = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_VALVE0                                                     */
/************************************************************************/
void app_read_REG_MIMIC_VALVE0(void)
{
	//app_regs.REG_MIMIC_VALVE0 = 0;

}

bool app_write_REG_MIMIC_VALVE0(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_VALVE0 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_VALVE1                                                     */
/************************************************************************/
void app_read_REG_MIMIC_VALVE1(void)
{
	//app_regs.REG_MIMIC_VALVE1 = 0;

}

bool app_write_REG_MIMIC_VALVE1(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_VALVE1 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_VALVE2                                                     */
/************************************************************************/
void app_read_REG_MIMIC_VALVE2(void)
{
	//app_regs.REG_MIMIC_VALVE2 = 0;

}

bool app_write_REG_MIMIC_VALVE2(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_VALVE2 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_VALVE3                                                     */
/************************************************************************/
void app_read_REG_MIMIC_VALVE3(void)
{
	//app_regs.REG_MIMIC_VALVE3 = 0;

}

bool app_write_REG_MIMIC_VALVE3(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_VALVE3 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_ENDVALVE0                                                  */
/************************************************************************/
void app_read_REG_MIMIC_ENDVALVE0(void)
{
	//app_regs.REG_MIMIC_ENDVALVE0 = 0;

}

bool app_write_REG_MIMIC_ENDVALVE0(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_ENDVALVE0 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_VALVE_AUX1                                                  */
/************************************************************************/
void app_read_REG_MIMIC_ENDVALVE1(void)
{
	//app_regs.REG_MIMIC_ENDVALVE1 = 0;

}

bool app_write_REG_MIMIC_ENDVALVE1(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_ENDVALVE1 = reg;
	return true;
}


/************************************************************************/
/* REG_MIMIC_DUMMYVALVE                                                 */
/************************************************************************/
void app_read_REG_MIMIC_DUMMYVALVE(void)
{
	//app_regs.REG_MIMIC_DUMMYVALVE = 0;

}

bool app_write_REG_MIMIC_DUMMYVALVE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_MIMIC_DUMMYVALVE = reg;
	return true;
}

/************************************************************************/
/* REG_EXT_CTRL_VALVES                                                  */
/************************************************************************/
void app_read_REG_EXT_CTRL_VALVES(void)
{
	//app_regs.REG_EXT_CTRL_VALVES = 0;

}

bool app_write_REG_EXT_CTRL_VALVES(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_EXT_CTRL_VALVES = reg;
	return true;
}


/************************************************************************/
/* REG_CHANNEL3_RANGE                                                   */
/************************************************************************/
void app_read_REG_CHANNEL3_RANGE(void)
{
	//app_regs.REG_CHANNEL3_RANGE = 0;

}

bool app_write_REG_CHANNEL3_RANGE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_CHANNEL3_RANGE = reg;
	
	float reg_ch3_flow_target = app_regs.REG_CHANNEL3_FLOW_TARGET;
	
	if((app_regs.REG_CHANNEL3_RANGE & MSK_CHANNEL3_RANGE_CONFIG) == GM_FLOW_100){
		if (reg_ch3_flow_target > 110){
			reg_ch3_flow_target = 110;
			//app_write_REG_CHANNEL3_FLOW_TARGET(&reg_ch3_flow_target);
		}
	}
	/*else{
		if (reg_ch3_flow_target > 1100){
			reg_ch3_flow_target = 1100;
			app_write_REG_CHANNEL3_FLOW_TARGET(&reg_ch3_flow_target)
		}
	}*/
	
	app_write_REG_CHANNEL3_FLOW_TARGET(&reg_ch3_flow_target);
	
	init_calibration_values();
		
	return true;
}


/************************************************************************/
/* REG_RESERVED0                                                        */
/************************************************************************/
void app_read_REG_RESERVED0(void)
{
	//app_regs.REG_RESERVED0 = 0;

}

bool app_write_REG_RESERVED0(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED0 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED1                                                        */
/************************************************************************/
void app_read_REG_RESERVED1(void)
{
	//app_regs.REG_RESERVED1 = 0;

}

bool app_write_REG_RESERVED1(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED1 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED2                                                        */
/************************************************************************/
void app_read_REG_RESERVED2(void)
{
	//app_regs.REG_RESERVED2 = 0;

}

bool app_write_REG_RESERVED2(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED2 = reg;
	return true;
}


/************************************************************************/
/* REG_ENABLE_EVENTS                                                      */
/************************************************************************/
void app_read_REG_ENABLE_EVENTS(void)
{
	//app_regs.REG_ENABLE_EVENTS = 0;

}

bool app_write_REG_ENABLE_EVENTS(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_ENABLE_EVENTS = reg;
	return true;
}
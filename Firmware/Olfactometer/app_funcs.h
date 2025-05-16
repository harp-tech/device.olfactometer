#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
#define bool uint8_t
#endif
#ifndef true
#define true 1
#endif
#ifndef false
#define false 0
#endif

#define start_VALVE0 do {set_VALVE0; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE0) pulse_countdown.valve0 = app_regs.REG_VALVE0_PULSE_DURATION + 1;\
if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE0) pulse_countdown.delayvalve0chk = app_regs.REG_VALVE0CHK_DELAY + 1;} while(0)
#define start_VALVE1 do {set_VALVE1; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE1) pulse_countdown.valve1 = app_regs.REG_VALVE1_PULSE_DURATION + 1;\
if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE1) pulse_countdown.delayvalve1chk = app_regs.REG_VALVE1CHK_DELAY + 1;} while(0)
#define start_VALVE2 do {set_VALVE2; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE2) pulse_countdown.valve2 = app_regs.REG_VALVE2_PULSE_DURATION + 1;\
if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE2) pulse_countdown.delayvalve2chk = app_regs.REG_VALVE2CHK_DELAY + 1;} while(0)
#define start_VALVE3 do {set_VALVE3; if (app_regs.REG_ENABLE_VALVES_PULSE & B_VALVE3) pulse_countdown.valve3 = app_regs.REG_VALVE3_PULSE_DURATION + 1;\
if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE3) pulse_countdown.delayvalve3chk = app_regs.REG_VALVE3CHK_DELAY + 1;} while(0)

#define stop_VALVE0 do {clr_VALVE0; if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE0) pulse_countdown.delayvalve0chk = app_regs.REG_VALVE0CHK_DELAY + 1;} while(0)
#define stop_VALVE1 do {clr_VALVE1; if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE1) pulse_countdown.delayvalve1chk = app_regs.REG_VALVE1CHK_DELAY + 1;} while(0)
#define stop_VALVE2 do {clr_VALVE2; if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE2) pulse_countdown.delayvalve2chk = app_regs.REG_VALVE2CHK_DELAY + 1;} while(0)
#define stop_VALVE3 do {clr_VALVE3; if(app_regs.REG_ENABLE_CHECK_VALVES_SYNC & B_CHECK_VALVE3) pulse_countdown.delayvalve3chk = app_regs.REG_VALVE3CHK_DELAY + 1;} while(0)

#define start_VALVE0CHK do {set_VALVE0CHK; if (app_regs.REG_ENABLE_VALVES_PULSE & B_CHECK_VALVE0) pulse_countdown.chkvalve0 = app_regs.REG_VALVE0CHK_DELAY + 1;} while(0)
#define start_VALVE1CHK do {set_VALVE1CHK; if (app_regs.REG_ENABLE_VALVES_PULSE & B_CHECK_VALVE1) pulse_countdown.chkvalve1 = app_regs.REG_VALVE1CHK_DELAY + 1; } while(0)
#define start_VALVE2CHK do {set_VALVE2CHK; if (app_regs.REG_ENABLE_VALVES_PULSE & B_CHECK_VALVE2) pulse_countdown.chkvalve2 = app_regs.REG_VALVE2CHK_DELAY + 1; } while(0)
#define start_VALVE3CHK do {set_VALVE3CHK; if (app_regs.REG_ENABLE_VALVES_PULSE & B_CHECK_VALVE3) pulse_countdown.chkvalve3 = app_regs.REG_VALVE3CHK_DELAY + 1; } while(0)

#define start_VALVEAUX0 do {set_ENDVALVE0; if (app_regs.REG_ENABLE_VALVES_PULSE & B_ENDVALVE0) pulse_countdown.valveaux0 = app_regs.REG_END_VALVE0_PULSE_DURATION + 1; } while(0)
#define start_VALVEAUX1 do {set_ENDVALVE1; if (app_regs.REG_ENABLE_VALVES_PULSE & B_ENDVALVE1) pulse_countdown.valveaux1 = app_regs.REG_END_VALVE1_PULSE_DURATION + 1; } while(0)
#define start_VALVEDUMMY do {set_DUMMYVALVE; if (app_regs.REG_ENABLE_VALVES_PULSE & B_DUMMYVALVE) pulse_countdown.valvedummy = app_regs.REG_DUMMY_VALVE_PULSE_DURATION + 1; } while(0)

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_ENABLE_FLOW(void);
void app_read_REG_FLOWMETER_ANALOG_OUTPUTS(void);
void app_read_REG_DI0_STATE(void);
void app_read_REG_CHANNEL0_USER_CALIBRATION(void);
void app_read_REG_CHANNEL1_USER_CALIBRATION(void);
void app_read_REG_CHANNEL2_USER_CALIBRATION(void);
void app_read_REG_CHANNEL3_USER_CALIBRATION(void);
void app_read_REG_CHANNEL4_USER_CALIBRATION(void);
void app_read_REG_CHANNEL3_USER_CALIBRATION_AUX(void);
void app_read_REG_USER_CALIBRATION_ENABLE(void);
void app_read_REG_CHANNEL0_TARGET_FLOW(void);
void app_read_REG_CHANNEL1_TARGET_FLOW(void);
void app_read_REG_CHANNEL2_TARGET_FLOW(void);
void app_read_REG_CHANNEL3_TARGET_FLOW(void);
void app_read_REG_CHANNEL4_TARGET_FLOW(void);
void app_read_REG_CHANNELS_TARGET_FLOW(void);
void app_read_REG_CHANNEL0_ACTUAL_FLOW(void);
void app_read_REG_CHANNEL1_ACTUAL_FLOW(void);
void app_read_REG_CHANNEL2_ACTUAL_FLOW(void);
void app_read_REG_CHANNEL3_ACTUAL_FLOW(void);
void app_read_REG_CHANNEL4_ACTUAL_FLOW(void);
void app_read_REG_CHANNEL0_FREQUENCY(void);
void app_read_REG_CHANNEL1_FREQUENCY(void);
void app_read_REG_CHANNEL2_FREQUENCY(void);
void app_read_REG_CHANNEL3_FREQUENCY(void);
void app_read_REG_CHANNEL4_FREQUENCY(void);
void app_read_REG_CHANNEL0_DUTY_CYCLE(void);
void app_read_REG_CHANNEL1_DUTY_CYCLE(void);
void app_read_REG_CHANNEL2_DUTY_CYCLE(void);
void app_read_REG_CHANNEL3_DUTY_CYCLE(void);
void app_read_REG_CHANNEL4_DUTY_CYCLE(void);
void app_read_REG_OUTPUT_SET(void);
void app_read_REG_OUTPUT_CLEAR(void);
void app_read_REG_OUTPUT_TOGGLE(void);
void app_read_REG_OUTPUT_STATE(void);
void app_read_REG_ENABLE_VALVES_PULSE(void);
void app_read_REG_VALVES_SET(void);
void app_read_REG_VALVES_CLEAR(void);
void app_read_REG_VALVES_TOGGLE(void);
void app_read_REG_VALVES_STATE(void);
void app_read_REG_ODOR_VALVES_STATE(void);
void app_read_REG_END_VALVES_STATE(void);
void app_read_REG_CHECK_VALVES_STATE(void);
void app_read_REG_VALVE0_PULSE_DURATION(void);
void app_read_REG_VALVE1_PULSE_DURATION(void);
void app_read_REG_VALVE2_PULSE_DURATION(void);
void app_read_REG_VALVE3_PULSE_DURATION(void);
void app_read_REG_VALVE0CHK_DELAY(void);
void app_read_REG_VALVE1CHK_DELAY(void);
void app_read_REG_VALVE2CHK_DELAY(void);
void app_read_REG_VALVE3CHK_DELAY(void);
void app_read_REG_END_VALVE0_PULSE_DURATION(void);
void app_read_REG_END_VALVE1_PULSE_DURATION(void);
void app_read_REG_DUMMY_VALVE_PULSE_DURATION(void);
void app_read_REG_DO0_SYNC(void);
void app_read_REG_DO1_SYNC(void);
void app_read_REG_DI0_TRIGGER(void);
void app_read_REG_MIMIC_ODOR_VALVE0(void);
void app_read_REG_MIMIC_ODOR_VALVE1(void);
void app_read_REG_MIMIC_ODOR_VALVE2(void);
void app_read_REG_MIMIC_ODOR_VALVE3(void);
void app_read_REG_MIMIC_CHECK_VALVE0(void);
void app_read_REG_MIMIC_CHECK_VALVE1(void);
void app_read_REG_MIMIC_CHECK_VALVE2(void);
void app_read_REG_MIMIC_CHECK_VALVE3(void);
void app_read_REG_MIMIC_END_VALVE0(void);
void app_read_REG_MIMIC_END_VALVE1(void);
void app_read_REG_MIMIC_DUMMY_VALVE(void);
void app_read_REG_ENABLE_VALVE_EXT_CTRL(void);
void app_read_REG_CHANNEL3_RANGE(void);
void app_read_REG_ENABLE_CHECK_VALVES_SYNC(void);
void app_read_REG_TEMPERATURE_VALUE(void);
void app_read_REG_ENABLE_TEMP_CALIBRATION(void);
void app_read_REG_TEMP_USER_CALIBRATION(void);
void app_read_REG_ENABLE_EVENTS(void);

bool app_write_REG_ENABLE_FLOW(void *a);
bool app_write_REG_FLOWMETER_ANALOG_OUTPUTS(void *a);
bool app_write_REG_DI0_STATE(void *a);
bool app_write_REG_CHANNEL0_USER_CALIBRATION(void *a);
bool app_write_REG_CHANNEL1_USER_CALIBRATION(void *a);
bool app_write_REG_CHANNEL2_USER_CALIBRATION(void *a);
bool app_write_REG_CHANNEL3_USER_CALIBRATION(void *a);
bool app_write_REG_CHANNEL4_USER_CALIBRATION(void *a);
bool app_write_REG_CHANNEL3_USER_CALIBRATION_AUX(void *a);
bool app_write_REG_USER_CALIBRATION_ENABLE(void *a);
bool app_write_REG_CHANNEL0_TARGET_FLOW(void *a);
bool app_write_REG_CHANNEL1_TARGET_FLOW(void *a);
bool app_write_REG_CHANNEL2_TARGET_FLOW(void *a);
bool app_write_REG_CHANNEL3_TARGET_FLOW(void *a);
bool app_write_REG_CHANNEL4_TARGET_FLOW(void *a);
bool app_write_REG_CHANNELS_TARGET_FLOW(void *a);
bool app_write_REG_CHANNEL0_ACTUAL_FLOW(void *a);
bool app_write_REG_CHANNEL1_ACTUAL_FLOW(void *a);
bool app_write_REG_CHANNEL2_ACTUAL_FLOW(void *a);
bool app_write_REG_CHANNEL3_ACTUAL_FLOW(void *a);
bool app_write_REG_CHANNEL4_ACTUAL_FLOW(void *a);
bool app_write_REG_CHANNEL0_FREQUENCY(void *a);
bool app_write_REG_CHANNEL1_FREQUENCY(void *a);
bool app_write_REG_CHANNEL2_FREQUENCY(void *a);
bool app_write_REG_CHANNEL3_FREQUENCY(void *a);
bool app_write_REG_CHANNEL4_FREQUENCY(void *a);
bool app_write_REG_CHANNEL0_DUTY_CYCLE(void *a);
bool app_write_REG_CHANNEL1_DUTY_CYCLE(void *a);
bool app_write_REG_CHANNEL2_DUTY_CYCLE(void *a);
bool app_write_REG_CHANNEL3_DUTY_CYCLE(void *a);
bool app_write_REG_CHANNEL4_DUTY_CYCLE(void *a);
bool app_write_REG_OUTPUT_SET(void *a);
bool app_write_REG_OUTPUT_CLEAR(void *a);
bool app_write_REG_OUTPUT_TOGGLE(void *a);
bool app_write_REG_OUTPUT_STATE(void *a);
bool app_write_REG_ENABLE_VALVES_PULSE(void *a);
bool app_write_REG_VALVES_SET(void *a);
bool app_write_REG_VALVES_CLEAR(void *a);
bool app_write_REG_VALVES_TOGGLE(void *a);
bool app_write_REG_VALVES_STATE(void *a);
bool app_write_REG_ODOR_VALVES_STATE(void *a);
bool app_write_REG_END_VALVES_STATE(void *a);
bool app_write_REG_CHECK_VALVES_STATE(void *a);
bool app_write_REG_VALVE0_PULSE_DURATION(void *a);
bool app_write_REG_VALVE1_PULSE_DURATION(void *a);
bool app_write_REG_VALVE2_PULSE_DURATION(void *a);
bool app_write_REG_VALVE3_PULSE_DURATION(void *a);
bool app_write_REG_VALVE0CHK_DELAY(void *a);
bool app_write_REG_VALVE1CHK_DELAY(void *a);
bool app_write_REG_VALVE2CHK_DELAY(void *a);
bool app_write_REG_VALVE3CHK_DELAY(void *a);
bool app_write_REG_END_VALVE0_PULSE_DURATION(void *a);
bool app_write_REG_END_VALVE1_PULSE_DURATION(void *a);
bool app_write_REG_DUMMY_VALVE_PULSE_DURATION(void *a);
bool app_write_REG_DO0_SYNC(void *a);
bool app_write_REG_DO1_SYNC(void *a);
bool app_write_REG_DI0_TRIGGER(void *a);
bool app_write_REG_MIMIC_ODOR_VALVE0(void *a);
bool app_write_REG_MIMIC_ODOR_VALVE1(void *a);
bool app_write_REG_MIMIC_ODOR_VALVE2(void *a);
bool app_write_REG_MIMIC_ODOR_VALVE3(void *a);
bool app_write_REG_MIMIC_CHECK_VALVE0(void *a);
bool app_write_REG_MIMIC_CHECK_VALVE1(void *a);
bool app_write_REG_MIMIC_CHECK_VALVE2(void *a);
bool app_write_REG_MIMIC_CHECK_VALVE3(void *a);
bool app_write_REG_MIMIC_END_VALVE0(void *a);
bool app_write_REG_MIMIC_END_VALVE1(void *a);
bool app_write_REG_MIMIC_DUMMY_VALVE(void *a);
bool app_write_REG_ENABLE_VALVE_EXT_CTRL(void *a);
bool app_write_REG_CHANNEL3_RANGE(void *a);
bool app_write_REG_ENABLE_CHECK_VALVES_SYNC(void *a);
bool app_write_REG_TEMPERATURE_VALUE(void *a);
bool app_write_REG_ENABLE_TEMP_CALIBRATION(void *a);
bool app_write_REG_TEMP_USER_CALIBRATION(void *a);
bool app_write_REG_ENABLE_EVENTS(void *a);


#endif /* _APP_FUNCTIONS_H_ */
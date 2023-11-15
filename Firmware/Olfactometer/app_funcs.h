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
void app_read_REG_ISOLATION_VALVES_STATE(void);
void app_read_REG_END_VALVES_STATE(void);
void app_read_REG_VALVE0_PULSE_DURATION(void);
void app_read_REG_VALVE1_PULSE_DURATION(void);
void app_read_REG_VALVE2_PULSE_DURATION(void);
void app_read_REG_VALVE3_PULSE_DURATION(void);
void app_read_REG_END_VALVE0_PULSE_DURATION(void);
void app_read_REG_END_VALVE1_PULSE_DURATION(void);
void app_read_REG_DUMMY_VALVE_PULSE_DURATION(void);
void app_read_REG_DO0_SYNC(void);
void app_read_REG_DO1_SYNC(void);
void app_read_REG_DI0_TRIGGER(void);
void app_read_REG_MIMIC_VALVE0(void);
void app_read_REG_MIMIC_VALVE1(void);
void app_read_REG_MIMIC_VALVE2(void);
void app_read_REG_MIMIC_VALVE3(void);
void app_read_REG_MIMIC_END_VALVE0(void);
void app_read_REG_MIMIC_END_VALVE1(void);
void app_read_REG_MIMIC_DUMMY_VALVE(void);
void app_read_REG_ENABLE_VALVE_EXT_CTRL(void);
void app_read_REG_CHANNEL3_RANGE(void);
void app_read_REG_RESERVED0(void);
void app_read_REG_RESERVED1(void);
void app_read_REG_RESERVED2(void);
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
bool app_write_REG_ISOLATION_VALVES_STATE(void *a);
bool app_write_REG_END_VALVES_STATE(void *a);
bool app_write_REG_VALVE0_PULSE_DURATION(void *a);
bool app_write_REG_VALVE1_PULSE_DURATION(void *a);
bool app_write_REG_VALVE2_PULSE_DURATION(void *a);
bool app_write_REG_VALVE3_PULSE_DURATION(void *a);
bool app_write_REG_END_VALVE0_PULSE_DURATION(void *a);
bool app_write_REG_END_VALVE1_PULSE_DURATION(void *a);
bool app_write_REG_DUMMY_VALVE_PULSE_DURATION(void *a);
bool app_write_REG_DO0_SYNC(void *a);
bool app_write_REG_DO1_SYNC(void *a);
bool app_write_REG_DI0_TRIGGER(void *a);
bool app_write_REG_MIMIC_VALVE0(void *a);
bool app_write_REG_MIMIC_VALVE1(void *a);
bool app_write_REG_MIMIC_VALVE2(void *a);
bool app_write_REG_MIMIC_VALVE3(void *a);
bool app_write_REG_MIMIC_END_VALVE0(void *a);
bool app_write_REG_MIMIC_END_VALVE1(void *a);
bool app_write_REG_MIMIC_DUMMY_VALVE(void *a);
bool app_write_REG_ENABLE_VALVE_EXT_CTRL(void *a);
bool app_write_REG_CHANNEL3_RANGE(void *a);
bool app_write_REG_RESERVED0(void *a);
bool app_write_REG_RESERVED1(void *a);
bool app_write_REG_RESERVED2(void *a);
bool app_write_REG_ENABLE_EVENTS(void *a);


#endif /* _APP_FUNCTIONS_H_ */
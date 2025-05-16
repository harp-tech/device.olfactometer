#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
void mimic_valves (uint8_t reg, uint8_t function);

#define _SET_IO_ 0
#define _CLR_IO_ 1
#define _TGL_IO_ 2

/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// IN0                    Description: Input IN0
// VERSION_CTRL           Description: HW version control
// MISO                   Description: ADC SPI serial data output
// BUSY                   Description: ADC busy
// VALVE0CTRL             Description: Valve 0 external control signal
// VALVE1CTRL             Description: Valve 1 external control signal
// VALVE2CTRL             Description: Valve 2 external control signal
// VALVE3CTRL             Description: Valve 3 external control signal
// VALVE0CHKCTRL          Description: Valve 0 check external control signal
// VALVE1CHKCTRL          Description: Valve 1 check external control signal
// VALVE2CHKCTRL          Description: Valve 2 check external control signal
// VALVE3CHKCTRL          Description: Valve 3 check external control signal
// ENDVALVECTRL           Description: End valve external control signal
// FLUSHVALVECTRL         Description: Flush valve external control signal

#define read_IN0 read_io(PORTB, 0)              // IN0
#define read_VERSION_CTRL read_io(PORTK, 7)     // VERSION_CTRL
#define read_MISO read_io(PORTE, 6)             // MISO
#define read_BUSY read_io(PORTH, 1)             // BUSY
#define read_VALVE0CTRL read_io(PORTA, 0)       // VALVE0CTRL
#define read_VALVE1CTRL read_io(PORTA, 1)       // VALVE1CTRL
#define read_VALVE2CTRL read_io(PORTA, 2)       // VALVE2CTRL
#define read_VALVE3CTRL read_io(PORTA, 3)       // VALVE3CTRL
#define read_VALVE0CHKCTRL read_io(PORTK, 4)    // VALVE0CHKCTRL
#define read_VALVE1CHKCTRL read_io(PORTK, 6)    // VALVE1CHKCTRL
#define read_VALVE2CHKCTRL read_io(PORTK, 1)    // VALVE2CHKCTRL
#define read_VALVE3CHKCTRL read_io(PORTK, 5)    // VALVE3CHKCTRL
#define read_ENDVALVECTRL read_io(PORTA, 4)     // ENDVALVECTRL
#define read_FLUSHVALVECTRL read_io(PORTA, 5)   // FLUSHVALVE

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// OUT0                   Description: Output DOUT0
// OUT1                   Description: Output DOUT1
// PWM0                   Description: Proportional valve 0 PWM signal
// PWM1                   Description: Proportional valve 1 PWM signal
// PWM2                   Description: Proportional valve 2 PWM signal
// PWM3                   Description: Proportional valve 3 PWM signal
// PWM4                   Description: Proportional valve 4 PWM signal
// VALVE0                 Description: Valve 0 control signal
// VALVE1                 Description: Valve 1 control signal
// VALVE2                 Description: Valve 2 control signal
// VALVE3                 Description: Valve 3 control signal
// CS_ADC                 Description: ADC SPI Chip select
// CONVST                 Description: ADC conversion start
// MOSI                   Description: ADC SPI serial data input
// SCK                    Description: ADC SPI serial data clock
// RESET                  Description: ADC reset
// DUMMY0                 Description: Dummy 0
// DUMMYVALVE             Description: Valve aux dummy input control signal
// ENDVALVE0              Description: Valve Aux 0 input control signal
// ENDVALVE1              Description: Valve Aux 1 input control signal

/* OUT0 */
#define set_OUT0 set_io(PORTH, 4)
#define clr_OUT0 clear_io(PORTH, 4)
#define tgl_OUT0 toggle_io(PORTH, 4)
#define read_OUT0 read_io(PORTH, 4)

/* OUT1 */
#define set_OUT1 set_io(PORTH, 5)
#define clr_OUT1 clear_io(PORTH, 5)
#define tgl_OUT1 toggle_io(PORTH, 5)
#define read_OUT1 read_io(PORTH, 5)

/* PWM0 */
#define set_PWM0 set_io(PORTC, 0)
#define clr_PWM0 clear_io(PORTC, 0)
#define tgl_PWM0 toggle_io(PORTC, 0)
#define read_PWM0 read_io(PORTC, 0)

/* PWM1 */
#define set_PWM1 set_io(PORTD, 0)
#define clr_PWM1 clear_io(PORTD, 0)
#define tgl_PWM1 toggle_io(PORTD, 0)
#define read_PWM1 read_io(PORTD, 0)

/* PWM2 */
#define set_PWM2 set_io(PORTE, 0)
#define clr_PWM2 clear_io(PORTE, 0)
#define tgl_PWM2 toggle_io(PORTE, 0)
#define read_PWM2 read_io(PORTE, 0)

/* PWM3 */
#define set_PWM3 set_io(PORTF, 0)
#define clr_PWM3 clear_io(PORTF, 0)
#define tgl_PWM3 toggle_io(PORTF, 0)
#define read_PWM3 read_io(PORTF, 0)

/* PWM4 */
#define set_PWM4 set_io(PORTD, 4)
#define clr_PWM4 clear_io(PORTD, 4)
#define tgl_PWM4 toggle_io(PORTD, 4)
#define read_PWM4 read_io(PORTD, 4)

/* VALVE0 */
#define set_VALVE0 do { set_io(PORTC, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE0, _SET_IO_); } while(0)
#define clr_VALVE0 do { clear_io(PORTC, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE0, _CLR_IO_); } while(0)
#define tgl_VALVE0 do { toggle_io(PORTC, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE0, _TGL_IO_); } while(0)
#define read_VALVE0 read_io(PORTC, 1)

/* VALVE1 */
#define set_VALVE1 do { set_io(PORTD, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE1, _SET_IO_); } while(0)
#define clr_VALVE1 do { clear_io(PORTD, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE1, _CLR_IO_); } while(0)
#define tgl_VALVE1 do { toggle_io(PORTD, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE1, _TGL_IO_); } while(0)
#define read_VALVE1 read_io(PORTD, 1)

/* VALVE2 */
#define set_VALVE2 do { set_io(PORTE, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE2, _SET_IO_); } while(0)
#define clr_VALVE2 do { clear_io(PORTE, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE2, _CLR_IO_); } while(0)
#define tgl_VALVE2 do { toggle_io(PORTE, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE2, _TGL_IO_); } while(0)
#define read_VALVE2 read_io(PORTE, 1)

/* VALVE3 */
#define set_VALVE3 do { set_io(PORTF, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE3, _SET_IO_); } while(0)
#define clr_VALVE3 do { clear_io(PORTF, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE3, _CLR_IO_); } while(0)
#define tgl_VALVE3 do { toggle_io(PORTF, 1); mimic_valves(app_regs.REG_MIMIC_ODOR_VALVE3, _TGL_IO_); } while(0)
#define read_VALVE3 read_io(PORTF, 1)

/* VALVE0CHK */
#define set_VALVE0CHK do { set_io(PORTD, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE0, _SET_IO_); } while(0)
#define clr_VALVE0CHK do { clear_io(PORTD, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE0, _CLR_IO_); } while(0)
#define tgl_VALVE0CHK do { toggle_io(PORTD, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE0, _TGL_IO_); } while(0)
#define read_VALVE0CHK read_io(PORTD, 5)

/* VALVE1CHK */
#define set_VALVE1CHK do { set_io(PORTD, 7); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE1, _SET_IO_); } while(0)
#define clr_VALVE1CHK do { clear_io(PORTD, 7); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE1, _CLR_IO_); } while(0)
#define tgl_VALVE1CHK do { toggle_io(PORTD, 7); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE1, _TGL_IO_); } while(0)
#define read_VALVE1CHK read_io(PORTD, 7)

/* VALVE2CHK */
#define set_VALVE2CHK do { set_io(PORTE, 3); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE2, _SET_IO_); } while(0)
#define clr_VALVE2CHK do { clear_io(PORTE, 3); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE2, _CLR_IO_); } while(0)
#define tgl_VALVE2CHK do { toggle_io(PORTE, 3); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE2, _TGL_IO_); } while(0)
#define read_VALVE2CHK read_io(PORTE, 3)

/* VALVE3CHK */
#define set_VALVE3CHK do { set_io(PORTF, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE3, _SET_IO_); } while(0)
#define clr_VALVE3CHK do { clear_io(PORTF, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE3, _CLR_IO_); } while(0)
#define tgl_VALVE3CHK do { toggle_io(PORTF, 5); mimic_valves(app_regs.REG_MIMIC_CHECK_VALVE3, _TGL_IO_); } while(0)
#define read_VALVE3CHK read_io(PORTF, 5)

/* RE_DE_5V_0 */
#define set_RE_DE_5V_0 set_io(PORTB, 2)
#define clr_RE_DE_5V_0 clear_io(PORTB, 2)
#define tgl_RE_DE_5V_0 toggle_io(PORTB, 2)
#define read_RE_DE_5V_0 read_io(PORTB, 2)

/* RE_DE_5V_1 */
#define set_RE_DE_5V_1 set_io(PORTB, 3)
#define clr_RE_DE_5V_1 clear_io(PORTB, 3)
#define tgl_RE_DE_5V_1 toggle_io(PORTB, 3)
#define read_RE_DE_5V_1 read_io(PORTB, 3)

/* RE_DE_5V_2 */
#define set_RE_DE_5V_2 set_io(PORTB, 4)
#define clr_RE_DE_5V_2 clear_io(PORTB, 4)
#define tgl_RE_DE_5V_2 toggle_io(PORTB, 4)
#define read_RE_DE_5V_2 read_io(PORTB, 4)

/* RE_DE_5V_3 */
#define set_RE_DE_5V_3 set_io(PORTB, 5)
#define clr_RE_DE_5V_3 clear_io(PORTB, 5)
#define tgl_RE_DE_5V_3 toggle_io(PORTB, 5)
#define read_RE_DE_5V_3 read_io(PORTB, 5)

/* RE_DE_5V_4 */
#define set_RE_DE_5V_4 set_io(PORTB, 6)
#define clr_RE_DE_5V_4 clear_io(PORTB, 6)
#define tgl_RE_DE_5V_4 toggle_io(PORTB, 6)
#define read_RE_DE_5V_4 read_io(PORTB, 6)

/* CS_ADC */
#define set_CS_ADC clear_io(PORTE, 4)
#define clr_CS_ADC set_io(PORTE, 4)
#define tgl_CS_ADC toggle_io(PORTE, 4)
#define read_CS_ADC read_io(PORTE, 4)

/* CS_TEMP */
#define set_CS_TEMP clear_io(PORTE, 2)
#define clr_CS_TEMP set_io(PORTE, 2)
#define tgl_CS_TEMP toggle_io(PORTE, 2)
#define read_CS_TEMP read_io(PORTE, 2)

/* CONVST */
#define set_CONVST set_io(PORTF, 4)
#define clr_CONVST clear_io(PORTF, 4)
#define tgl_CONVST toggle_io(PORTF, 4)
#define read_CONVST read_io(PORTF, 4)

/* MOSI */
#define set_MOSI set_io(PORTE, 5)
#define clr_MOSI clear_io(PORTE, 5)
#define tgl_MOSI toggle_io(PORTE, 5)
#define read_MOSI read_io(PORTE, 5)

/* SCK */
#define set_SCK set_io(PORTE, 7)
#define clr_SCK clear_io(PORTE, 7)
#define tgl_SCK toggle_io(PORTE, 7)
#define read_SCK read_io(PORTE, 7)

/* RESET */
#define set_RESET set_io(PORTH, 0)
#define clr_RESET clear_io(PORTH, 0)
#define tgl_RESET toggle_io(PORTH, 0)
#define read_RESET read_io(PORTH, 0)

/* DUMMY0 */
#define set_DUMMY0 set_io(PORTJ, 0)
#define clr_DUMMY0 clear_io(PORTJ, 0)
#define tgl_DUMMY0 toggle_io(PORTJ, 0)
#define read_DUMMY0 read_io(PORTJ, 0)

/* DUMMYVALVE */
#define set_DUMMYVALVE do { set_io(PORTJ, 1); mimic_valves(app_regs.REG_MIMIC_DUMMY_VALVE, _SET_IO_); } while(0)
#define clr_DUMMYVALVE do { clear_io(PORTJ, 1); mimic_valves(app_regs.REG_MIMIC_DUMMY_VALVE, _CLR_IO_); } while(0)
#define tgl_DUMMYVALVE do { toggle_io(PORTJ, 1); mimic_valves(app_regs.REG_MIMIC_DUMMY_VALVE, _TGL_IO_); } while(0)
#define read_DUMMYVALVE read_io(PORTJ, 1)

/* ENDVALVE0 */
#define set_ENDVALVE0 do { set_io(PORTD, 2); mimic_valves(app_regs.REG_MIMIC_END_VALVE0, _SET_IO_); } while(0)
#define clr_ENDVALVE0 do { clear_io(PORTD, 2); mimic_valves(app_regs.REG_MIMIC_END_VALVE0, _CLR_IO_); } while(0)
#define tgl_ENDVALVE0 do { toggle_io(PORTD, 2); mimic_valves(app_regs.REG_MIMIC_END_VALVE0, _TGL_IO_); } while(0)
#define read_ENDVALVE0 read_io(PORTD, 2)

/* ENDVALVE1 */
#define set_ENDVALVE1 do { set_io(PORTD, 3); mimic_valves(app_regs.REG_MIMIC_END_VALVE1, _SET_IO_); } while(0)
#define clr_ENDVALVE1 do { clear_io(PORTD, 3); mimic_valves(app_regs.REG_MIMIC_END_VALVE1, _CLR_IO_); } while(0)
#define tgl_ENDVALVE1 do { toggle_io(PORTD, 3); mimic_valves(app_regs.REG_MIMIC_END_VALVE1, _TGL_IO_); } while(0)
#define read_ENDVALVE1 read_io(PORTD, 3)

/* RANGE */
#define set_RANGE set_io(PORTD, 6)
#define clr_RANGE clear_io(PORTD, 6)
#define tgl_RANGE toggle_io(PORTD, 6)
#define read_RANGE read_io(PORTD, 6)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_ENABLE_FLOW;
	int16_t REG_FLOWMETER_ANALOG_OUTPUTS[5];
	uint8_t REG_DI0_STATE;
	uint16_t REG_CHANNEL0_USER_CALIBRATION[11];
	uint16_t REG_CHANNEL1_USER_CALIBRATION[11];
	uint16_t REG_CHANNEL2_USER_CALIBRATION[11];
	uint16_t REG_CHANNEL3_USER_CALIBRATION[11];
	uint16_t REG_CHANNEL4_USER_CALIBRATION[11];
	uint16_t REG_CHANNEL3_USER_CALIBRATION_AUX[11];
	uint8_t REG_USER_CALIBRATION_ENABLE;
	float REG_CHANNEL0_TARGET_FLOW;
	float REG_CHANNEL1_TARGET_FLOW;
	float REG_CHANNEL2_TARGET_FLOW;
	float REG_CHANNEL3_TARGET_FLOW;
	float REG_CHANNEL4_TARGET_FLOW;
	float REG_CHANNELS_TARGET_FLOW[5];
	float REG_CHANNEL0_ACTUAL_FLOW;
	float REG_CHANNEL1_ACTUAL_FLOW;
	float REG_CHANNEL2_ACTUAL_FLOW;
	float REG_CHANNEL3_ACTUAL_FLOW;
	float REG_CHANNEL4_ACTUAL_FLOW;
	uint16_t REG_CHANNEL0_FREQUENCY;
	uint16_t REG_CHANNEL1_FREQUENCY;
	uint16_t REG_CHANNEL2_FREQUENCY;
	uint16_t REG_CHANNEL3_FREQUENCY;
	uint16_t REG_CHANNEL4_FREQUENCY;
	float REG_CHANNEL0_DUTY_CYCLE;
	float REG_CHANNEL1_DUTY_CYCLE;
	float REG_CHANNEL2_DUTY_CYCLE;
	float REG_CHANNEL3_DUTY_CYCLE;
	float REG_CHANNEL4_DUTY_CYCLE;
	uint8_t REG_OUTPUT_SET;
	uint8_t REG_OUTPUT_CLEAR;
	uint8_t REG_OUTPUT_TOGGLE;
	uint8_t REG_OUTPUT_STATE;
	uint16_t REG_ENABLE_VALVES_PULSE;
	uint16_t REG_VALVES_SET;
	uint16_t REG_VALVES_CLEAR;
	uint16_t REG_VALVES_TOGGLE;
	uint16_t REG_VALVES_STATE;
	uint8_t REG_ODOR_VALVES_STATE;
	uint8_t REG_END_VALVES_STATE;
	uint16_t REG_CHECK_VALVES_STATE;
	uint16_t REG_VALVE0_PULSE_DURATION;
	uint16_t REG_VALVE1_PULSE_DURATION;
	uint16_t REG_VALVE2_PULSE_DURATION;
	uint16_t REG_VALVE3_PULSE_DURATION;
	uint16_t REG_VALVE0CHK_DELAY;
	uint16_t REG_VALVE1CHK_DELAY;
	uint16_t REG_VALVE2CHK_DELAY;
	uint16_t REG_VALVE3CHK_DELAY;
	uint16_t REG_END_VALVE0_PULSE_DURATION;
	uint16_t REG_END_VALVE1_PULSE_DURATION;
	uint16_t REG_DUMMY_VALVE_PULSE_DURATION;
	uint8_t REG_DO0_SYNC;
	uint8_t REG_DO1_SYNC;
	uint8_t REG_DI0_TRIGGER;
	uint8_t REG_MIMIC_ODOR_VALVE0;
	uint8_t REG_MIMIC_ODOR_VALVE1;
	uint8_t REG_MIMIC_ODOR_VALVE2;
	uint8_t REG_MIMIC_ODOR_VALVE3;
	uint8_t REG_MIMIC_CHECK_VALVE0;
	uint8_t REG_MIMIC_CHECK_VALVE1;
	uint8_t REG_MIMIC_CHECK_VALVE2;
	uint8_t REG_MIMIC_CHECK_VALVE3;
	uint8_t REG_MIMIC_END_VALVE0;
	uint8_t REG_MIMIC_END_VALVE1;
	uint8_t REG_MIMIC_DUMMY_VALVE;
	uint8_t REG_ENABLE_VALVE_EXT_CTRL;
	uint8_t REG_CHANNEL3_RANGE;
	uint16_t REG_ENABLE_CHECK_VALVES_SYNC;
	uint8_t REG_TEMPERATURE_VALUE;
	uint8_t REG_ENABLE_TEMP_CALIBRATION;
	uint8_t REG_TEMP_USER_CALIBRATION;
	uint8_t REG_ENABLE_EVENTS;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_ENABLE_FLOW                 32 // U8     Write  any value above zero to start the flowmeter and zero to stop
#define ADD_REG_FLOWMETER_ANALOG_OUTPUTS    33 // I16    Value of the analog inputs
#define ADD_REG_DI0_STATE                   34 // U8     State of digital input 0 (DI0)
#define ADD_REG_CHANNEL0_USER_CALIBRATION   35 // U16    Values of calibration for channel 0 - flowmeter 0 [x0,x1, �, x10] [x= ADC raw value for 0-100 ml/min, step 10]
#define ADD_REG_CHANNEL1_USER_CALIBRATION   36 // U16    Values of calibration for channel 1 - flowmeter 1 [x0,x1, �, x10] [x= ADC raw value for 0-100 ml/min, step 10]
#define ADD_REG_CHANNEL2_USER_CALIBRATION   37 // U16    Values of calibration for channel 2 - flowmeter 2 [x0,x1, �, x10] [x= ADC raw value for 0-100 ml/min, step 10]
#define ADD_REG_CHANNEL3_USER_CALIBRATION   38 // U16    Values of calibration for channel 3 - flowmeter 3 [x0,x1, �, x10] [x= ADC raw value for 0-100 ml/min, step 10]
#define ADD_REG_CHANNEL4_USER_CALIBRATION   39 // U16    Values of calibration for channel 4 - flowmeter 4 [x0,x1, �, x10] [x= ADC raw value for 0-1000 ml/min, step 100]
#define ADD_REG_CHANNEL3_USER_CALIBRATION_AUX 40 // U16    Values of calibration for channel 3 - flowmeter 3 [x0,x1, �, x10] [x= ADC raw value for 0-1000 ml/min, step 100]
#define ADD_REG_USER_CALIBRATION_ENABLE     41 // U8     Override the factory calibration values, replacing with CHX_USER_CALIBRATION
#define ADD_REG_CHANNEL0_TARGET_FLOW        42 // FLOAT  Flow value set for channel 0 - flowmeter 0 [ml/min]
#define ADD_REG_CHANNEL1_TARGET_FLOW        43 // FLOAT  Flow value set for channel 1 - flowmeter 1 [ml/min]
#define ADD_REG_CHANNEL2_TARGET_FLOW        44 // FLOAT  Flow value set for channel 2 - flowmeter 2 [ml/min]
#define ADD_REG_CHANNEL3_TARGET_FLOW        45 // FLOAT  Flow value set for channel 3 - flowmeter 3 [ml/min]
#define ADD_REG_CHANNEL4_TARGET_FLOW        46 // FLOAT  Flow value set for channel 4 - flowmeter 4 [ml/min]
#define ADD_REG_CHANNELS_TARGET_FLOW        47 // FLOAT  Flow value set for all channels [ml/min]
#define ADD_REG_CHANNEL0_ACTUAL_FLOW        48 // FLOAT  Flow value read from channel 0 - flowmeter 0 [ml/min]
#define ADD_REG_CHANNEL1_ACTUAL_FLOW        49 // FLOAT  Flow value read from channel 1 - flowmeter 1 [ml/min]
#define ADD_REG_CHANNEL2_ACTUAL_FLOW        50 // FLOAT  Flow value read from channel 2 - flowmeter 2 [ml/min]
#define ADD_REG_CHANNEL3_ACTUAL_FLOW        51 // FLOAT  Flow value read from channel 3 - flowmeter 3 [ml/min]
#define ADD_REG_CHANNEL4_ACTUAL_FLOW        52 // FLOAT  Flow value read from channel 4 - flowmeter 4 [ml/min]
#define ADD_REG_CHANNEL0_FREQUENCY          53 // U16    Switching frequency for proportional valve 0 - DO NOT CHANGE [Hz]
#define ADD_REG_CHANNEL1_FREQUENCY          54 // U16    Switching frequency for proportional valve 1 - DO NOT CHANGE [Hz]
#define ADD_REG_CHANNEL2_FREQUENCY          55 // U16    Switching frequency for proportional valve 2 - DO NOT CHANGE [Hz]
#define ADD_REG_CHANNEL3_FREQUENCY          56 // U16    Switching frequency for proportional valve 3 - DO NOT CHANGE [Hz]
#define ADD_REG_CHANNEL4_FREQUENCY          57 // U16    Switching frequency for proportional valve 4 - DO NOT CHANGE [Hz]
#define ADD_REG_CHANNEL0_DUTY_CYCLE         58 // FLOAT  Duty cycle for proportional valve 0 [%]
#define ADD_REG_CHANNEL1_DUTY_CYCLE         59 // FLOAT  Duty cycle for proportional valve 1 [%]
#define ADD_REG_CHANNEL2_DUTY_CYCLE         60 // FLOAT  Duty cycle for proportional valve 2 [%]
#define ADD_REG_CHANNEL3_DUTY_CYCLE         61 // FLOAT  Duty cycle for proportional valve 3 [%]
#define ADD_REG_CHANNEL4_DUTY_CYCLE         62 // FLOAT  Duty cycle for proportional valve 4 [%]
#define ADD_REG_OUTPUT_SET                  63 // U8     Set the correspondent output
#define ADD_REG_OUTPUT_CLEAR                64 // U8     Clear the correspondent output
#define ADD_REG_OUTPUT_TOGGLE               65 // U8     Toggle the correspondent output
#define ADD_REG_OUTPUT_STATE                66 // U8     Control the correspondent output
#define ADD_REG_ENABLE_VALVES_PULSE         67 // U16    Enable pulse mode for valves
#define ADD_REG_VALVES_SET                  68 // U16    Set the correspondent valve
#define ADD_REG_VALVES_CLEAR                69 // U16    Clear the correspondent valve
#define ADD_REG_VALVES_TOGGLE               70 // U16    Toggle the correspondent valve
#define ADD_REG_VALVES_STATE                71 // U16    Control the correspondent valve
#define ADD_REG_ODOR_VALVES_STATE           72 // U8     Control the correspondent odor valve
#define ADD_REG_END_VALVES_STATE            73 // U8     Control the correspondent end valve
#define ADD_REG_CHECK_VALVES_STATE          74 // U16    Control the correspondent check valve
#define ADD_REG_VALVE0_PULSE_DURATION       75 // U16    Valve 0 pulse duration [1:65535] ms
#define ADD_REG_VALVE1_PULSE_DURATION       76 // U16    Valve 1 pulse duration [1:65535] ms
#define ADD_REG_VALVE2_PULSE_DURATION       77 // U16    Valve 2 pulse duration [1:65535] ms
#define ADD_REG_VALVE3_PULSE_DURATION       78 // U16    Valve 3 pulse duration [1:65535] ms
#define ADD_REG_VALVE0CHK_DELAY             79 // U16    Check valve 0 delay [0:65535] ms
#define ADD_REG_VALVE1CHK_DELAY             80 // U16    Check valve 1 delay [0:65535] ms
#define ADD_REG_VALVE2CHK_DELAY             81 // U16    Check valve 2 delay [0:65535] ms
#define ADD_REG_VALVE3CHK_DELAY             82 // U16    Check valve 3 delay [0:65535] ms
#define ADD_REG_END_VALVE0_PULSE_DURATION   83 // U16    End valve 0 pulse duration [1:65535] ms
#define ADD_REG_END_VALVE1_PULSE_DURATION   84 // U16    End valve 1 pulse duration [1:65535] ms
#define ADD_REG_DUMMY_VALVE_PULSE_DURATION  85 // U16    Dummy valve pulse duration [1:65535] ms
#define ADD_REG_DO0_SYNC                    86 // U8     Configuration of digital output 0 (DOUT0)
#define ADD_REG_DO1_SYNC                    87 // U8     Configuration of digital output 1 (DOUT1)
#define ADD_REG_DI0_TRIGGER                 88 // U8     Configuration of the digital input 0 (DIN0)
#define ADD_REG_MIMIC_ODOR_VALVE0           89 // U8
#define ADD_REG_MIMIC_ODOR_VALVE1           90 // U8
#define ADD_REG_MIMIC_ODOR_VALVE2           91 // U8
#define ADD_REG_MIMIC_ODOR_VALVE3           92 // U8
#define ADD_REG_MIMIC_CHECK_VALVE0          93 // U8
#define ADD_REG_MIMIC_CHECK_VALVE1          94 // U8
#define ADD_REG_MIMIC_CHECK_VALVE2          95 // U8
#define ADD_REG_MIMIC_CHECK_VALVE3          96 // U8
#define ADD_REG_MIMIC_END_VALVE0            97 // U8
#define ADD_REG_MIMIC_END_VALVE1            98 // U8
#define ADD_REG_MIMIC_DUMMY_VALVE           99 // U8
#define ADD_REG_ENABLE_VALVE_EXT_CTRL      100 // U8     Write value 1 to control the valves via the screw terminals, write 0 through software
#define ADD_REG_CHANNEL3_RANGE             101 // U8     Selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min)
#define ADD_REG_ENABLE_CHECK_VALVES_SYNC   102 // U8     Configure the control mode of the isolation valves: 0: software, 1: sync with odor valves + delay
#define ADD_REG_TEMPERATURE_VALUE          103 // U8     Temperature sensor reading value
#define ADD_REG_ENABLE_TEMP_CALIBRATION    104 // U8     Enable flow adjustment based on the temperature calibration
#define ADD_REG_TEMP_CALIBRATION           105 // U8     Temperature value measured during the device calibration
#define ADD_REG_ENABLE_EVENTS              106 // U8     Enable the Events

/************************************************************************/
/* Olfactometer  registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x6A
#define APP_NBYTES_OF_REG_BANK              291

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_START                            (1<<0)       // Flowmeter running status
#define B_DIN0                             (1<<0)       // Digital input 0
#define B_DOUT0                            (1<<0)       // Digital output 0
#define B_DOUT1                            (1<<1)       // Digital output 1
#define B_VALVE0                           (1<<0)       // Valve 0
#define B_VALVE1                           (1<<1)       // Valve 1
#define B_VALVE2                           (1<<2)       // Valve 2
#define B_VALVE3                           (1<<3)       // Valve 3
#define B_ENDVALVE0                        (1<<4)       // End valve 0
#define B_ENDVALVE1                        (1<<5)       // End valve 1
#define B_DUMMYVALVE                       (1<<6)       // Dummy valve
#define B_CHECK_VALVE0                     (1<<8)       // Check valve 0 //(1<<4)
#define B_CHECK_VALVE1                     (1<<9)       // Check valve 1 //(1<<6) 
#define B_CHECK_VALVE2                     (1<<10)      // Check valve 2 //(1<<1) 
#define B_CHECK_VALVE3                     (1<<11)      // Check valve 3 //(1<<5)  
#define MSK_DOUT0_CONF                     (3<<0)       // Select DOUT0 function
#define GM_DOUT0_SOFTWARE                  (0<<0)       // Digital output 0 controlled by software
#define GM_DOUT0_START                     (1<<0)       // Equal to bit START
#define MSK_DOUT1_CONF                     (3<<0)       // Select DOUT1 function
#define GM_DOUT1_SOFTWARE                  (0<<0)       // Digital output 1 controlled by software
#define GM_DOUT1_START                     (1<<0)       // Equal to bit START
#define MSK_DIN0_CONF                      (3<<0)       // Select IN0 function
#define GM_DIN0_SYNC                       (0<<0)       // Config as regular digital input
#define GM_DIN0_RISE_START_FALL_STOP       (1<<0)       // On a rising edge will start the flowmeter and on a falling will stop it
#define GM_DIN0_VALVE_TOGGLE               (2<<0)       // Toggles the end valves with a rising and falling edge
#define MSK_MIMIC                          (3<<0)       //
#define GM_MIMIC_NONE                      (0<<0)       // No mimic is selected
#define GM_MIMIC_DO0                       (1<<0)       // Signal is reflected on DO0
#define GM_MIMIC_DO1                       (2<<0)       // Signal is reflected on DO1
#define B_EXT_CTRL_ENABLED                 (1<<0)       // Enabled the valves external control
#define MSK_CHANNEL3_RANGE_CONFIG          (3<<0)       // Available flow ranges for channel 3 (ml/min)
#define GM_FLOW_100                        (0<<0)       // Range is 0-100ml/min
#define GM_FLOW_1000                       (1<<0)       // Range is 0-1000ml/min
#define MSK_CHECK_VALVES_CONFIG            (3<<0)       // Check valves control configuration
#define B_EVT0                             (1<<0)       // Events of register FLOWMETER_ANALOG_OUTPUTS
#define B_EVT1                             (1<<1)       // Events of register DI0_STATE
#define B_EVT2                             (1<<2)       // Events of register CHANNELS_ACTUAL_FLOW

#endif /* _APP_REGS_H_ */
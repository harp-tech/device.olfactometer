#ifndef _AUX_FUNCS_H_
#define _AUX_FUNCS_H_


/************************************************************************/
/* Shared structures                                                    */
/************************************************************************/
typedef struct
{
	uint8_t DC0_ready, DC1_ready, DC2_ready, DC3_ready, DC4_ready;
	bool flow0_update, flow1_update, flow2_update, flow3_update, flow4_update;
	
} status_PWM_DC_t;

typedef struct
{
	uint16_t uart, valve0, valve1, valve2, valve3, valveaux0, valveaux1, valvedummy, chkvalve0, chkvalve1, chkvalve2, chkvalve3, delayvalve0chk, delayvalve1chk, delayvalve2chk, delayvalve3chk;
} countdown_t;


/************************************************************************/
/* Get decimal divider from prescaler                                   */
/************************************************************************/
uint16_t get_divider(uint8_t prescaler);

/************************************************************************/
/* Calculate PWM duty cycle value                                       */
/************************************************************************/
void hwbp_app_pwm_gen_update_dc0(void);
void hwbp_app_pwm_gen_update_dc1(void);
void hwbp_app_pwm_gen_update_dc2(void);
void hwbp_app_pwm_gen_update_dc3(void);
void hwbp_app_pwm_gen_update_dc4(void);

/************************************************************************/
/* Start PWM generation                                                 */
/************************************************************************/
uint8_t hwbp_app_pwm_gen_start_ch0(void);
uint8_t hwbp_app_pwm_gen_start_ch1(void);
uint8_t hwbp_app_pwm_gen_start_ch2(void);
uint8_t hwbp_app_pwm_gen_start_ch3(void);
uint8_t hwbp_app_pwm_gen_start_ch4(void);

/************************************************************************/
/* Stop PWM generation                                                  */
/************************************************************************/
uint8_t hwbp_app_pwm_gen_stop_ch0(void);
uint8_t hwbp_app_pwm_gen_stop_ch1(void);
uint8_t hwbp_app_pwm_gen_stop_ch2(void);
uint8_t hwbp_app_pwm_gen_stop_ch3(void);
uint8_t hwbp_app_pwm_gen_stop_ch4(void);

#endif /* _AUX_FUNCS_H_ */
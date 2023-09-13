#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"


/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

uint8_t aux = 0;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
//
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
//
// ISR(TCD1_OVF_vect, ISR_NAKED)
//
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/
/* External Valve Control Signals                                       */
/************************************************************************/

ISR(PORTA_INT0_vect, ISR_NAKED)
{
	// add equal to 
	if (app_regs.REG_EXT_CTRL_VALVES == B_EXT_CTRL_ENABLED){
		
		aux = ( ((read_ENDVALVECTRL) & 0x20) | ((read_ENDVALVECTRL) & 0x10) | ((read_VALVE3CTRL) & 0x08) | ((read_VALVE2CTRL) & 0x04) | ((read_VALVE1CTRL) & 0x02) | ((read_VALVE0CTRL) & 0x01) );
		
		app_write_REG_VALVES_STATE(&aux);
		
	}
	
	reti();
}


/************************************************************************/
/* Data ADC Busy Interrupt (CONVST-> BUSY)                              */
/************************************************************************/

ISR(PORTH_INT0_vect, ISR_NAKED)
{
	if (!read_BUSY)
	{
		clr_CONVST;
		set_CS_ADC;
		
		for (uint8_t i = 0; i < 5; i++) //run 5 ADC channels
		{
			SPIE_DATA = 0;
			loop_until_bit_is_set(SPIE_STATUS, SPI_IF_bp);
			*(((uint8_t*)(&app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[0])) + i*2 + 1) = SPIE_DATA;
			
			SPIE_DATA = 0;
			loop_until_bit_is_set(SPIE_STATUS, SPI_IF_bp);
			*(((uint8_t*)(&app_regs.REG_FLOWMETER_ANALOG_OUTPUTS[0])) + i*2 + 0) = SPIE_DATA;
		}
		
		clr_CS_ADC;
		
		if (app_regs.REG_ENABLE_EVENTS & B_EVT0)
		{
			core_func_send_event(ADD_REG_FLOWMETER_ANALOG_OUTPUTS, false);
		}
	}
	
	reti();
}


/************************************************************************/
/* IN00                                                                 */
/************************************************************************/
uint8_t previous_in0;

ISR(PORTB_INT0_vect, ISR_NAKED)
{
	uint8_t aux = read_IN0;
	
	app_regs.REG_DI0_STATE = aux;
	app_write_REG_DI0_STATE(&app_regs.REG_DI0_STATE);
	
	if (app_regs.REG_ENABLE_EVENTS & B_EVT1){
		// transition from low to high
		if(previous_in0 == 0 && aux == 1)
		{
			core_func_send_event(ADD_REG_DI0_STATE, true);
		}
		// transition from high to low
		else{
			core_func_send_event(ADD_REG_DI0_STATE, false);
		}
	}

	if((app_regs.REG_DI0_TRIGGER & MSK_DIN0_CONF) == GM_DIN0_VALVE_TOGGLE ){
	   
		if(previous_in0 == 0 && aux == 1)
		{
			set_ENDVALVE0;
			set_ENDVALVE1;
		} else {
			clr_ENDVALVE0;
			clr_ENDVALVE1;
		}
	}
    

	if((app_regs.REG_DI0_TRIGGER & MSK_DIN0_CONF) == GM_DIN0_SYNC ) {}
		
		
	if((app_regs.REG_DI0_TRIGGER & MSK_DIN0_CONF) == GM_DIN0_RISE_START_FALL_STOP )
	{
		// transition from low to high - start
		if(previous_in0 == 0 && aux == 1)
		{
			app_regs.REG_ENABLE_FLOW = aux;
			app_write_REG_ENABLE_FLOW(&app_regs.REG_ENABLE_FLOW);
			//core_func_send_event(ADD_REG_START, true);
		}
		// transition from high to low - stop
		else{
			app_regs.REG_ENABLE_FLOW = aux;
			app_write_REG_ENABLE_FLOW(&app_regs.REG_ENABLE_FLOW);
			//core_func_send_event(ADD_REG_START, false);
		}
	}
	
	previous_in0 = aux;
	
	reti();
}
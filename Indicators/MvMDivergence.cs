//
// Copyright (C) 2019, Machines Vs. Machines LLC.
//
#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class MvMDivergence : Indicator
	{
		/// CLASS GLOBAL CONSTANTS
		/// so we can param tune here
		private const int PERIOD       = 15;
		private const int SMOOTH       = 3;
		
		/// highest high/lowest low tracker
		private double    highestHigh;
		private double    lowestLow;
		double			  localHighestHigh = 0.00;
		double			  localLowestLow = 0.00;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter Later";
				Name										= "MvMDivergence";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;

				
				#if DEBUG
					PrintTo = PrintTo.OutputTab1;
					Print("MvMDivergence compiled with DEBUG flag. . .");
				#endif
			}
			else if (State == State.Configure)
			{
				//AddDataSeries("AMZN", Data.BarsPeriodType.Day, 15, Data.MarketDataType.Last);
			}		
		}
		protected override void OnBarUpdate()
		{
			if(CurrentBar < 5)
			{
				#if DEBUG
					Print("Skipping Bar " + CurrentBar);
				#endif
				return;
			}
			
			#if DEBUG
				Print("Current Bar: " + CurrentBar);
			#endif
			
			/// Calculate RSI on this bar
			double currentRSI   = RSI(PERIOD, SMOOTH)[0];
			double previousRSI  = RSI(PERIOD, SMOOTH)[1];
			double pastFiveRSI  = RSI(PERIOD, SMOOTH)[5];
			//double tenDayRSI = RSI(PERIOD, SMOOTH)[10];
			double tempRSI = 0.0;
			bool divergenceFound = true;
			//double yesterdayRSI = RSI(15, 3)[10];
			double lowestRSI    = 0.00;
			string CB = CurrentBar.ToString();
			/// Calculate "Highest High"
			if(currentRSI > highestHigh)
			{
				highestHigh = currentRSI;
			}
			
			/// Local Highest High
			if( currentRSI > pastFiveRSI )
			{
				localHighestHigh = currentRSI;
				//Draw.Diamond(this, "localHighestHigh" + CurrentBar.ToString(), true, 0, High[0] +1, Brushes.Blue, true);
			}
			
			for( int i = 5; i > 0; i-- )
			{
				tempRSI = RSI(PERIOD, SMOOTH)[i];
				if(tempRSI < localHighestHigh)
				{
					divergenceFound = false;
				}
				if(divergenceFound == true && tempRSI == localHighestHigh )
				{
					Draw.Diamond(this, "localHighestHigh" + CurrentBar.ToString(), true, 0, High[0] +1, Brushes.Blue, true);
				}
			}
			/*
			if(divergenceFound)
			{
				Draw.Diamond(this, "localHighestHigh" + CurrentBar.ToString(), true, 0, High[0] +1, Brushes.Blue, true);
			}
			*/
			#if DEBUG
				Print("  Global Highest High: " + highestHigh);
				Print("  Local Highest High: " + localHighestHigh);
				//Print("  TEST VAR: " + TESTVAR);
			#endif
			
			/// Calculate Lowest Low
			lowestRSI = lowestLow;
			if(currentRSI < lowestRSI)
			{
				lowestLow = currentRSI; //WTF WHY IS THIS DATA SO FUGG'D
			}
			
			/// Local lowest low
			if( currentRSI < pastFiveRSI )
			{
				localLowestLow = currentRSI;
			}

			#if DEBUG
				Print("  Global Lowest Low: " + lowestLow);
				Print("  Local Lowest Low: " + localLowestLow);
			#endif
			

			
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private MvMDivergence[] cacheMvMDivergence;
		public MvMDivergence MvMDivergence()
		{
			return MvMDivergence(Input);
		}

		public MvMDivergence MvMDivergence(ISeries<double> input)
		{
			if (cacheMvMDivergence != null)
				for (int idx = 0; idx < cacheMvMDivergence.Length; idx++)
					if (cacheMvMDivergence[idx] != null &&  cacheMvMDivergence[idx].EqualsInput(input))
						return cacheMvMDivergence[idx];
			return CacheIndicator<MvMDivergence>(new MvMDivergence(), input, ref cacheMvMDivergence);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.MvMDivergence MvMDivergence()
		{
			return indicator.MvMDivergence(Input);
		}

		public Indicators.MvMDivergence MvMDivergence(ISeries<double> input )
		{
			return indicator.MvMDivergence(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.MvMDivergence MvMDivergence()
		{
			return indicator.MvMDivergence(Input);
		}

		public Indicators.MvMDivergence MvMDivergence(ISeries<double> input )
		{
			return indicator.MvMDivergence(input);
		}
	}
}

#endregion

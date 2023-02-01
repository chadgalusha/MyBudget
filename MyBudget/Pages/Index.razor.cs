using PSC.Blazor.Components.Chartjs.Enums;
using PSC.Blazor.Components.Chartjs.Models.Common;
using PSC.Blazor.Components.Chartjs.Models.Bar;
using MyBudget.ViewModels;
using MyBudget.Helpers;
using Microsoft.AspNetCore.Components;
using MyBudget.Services;
using System.Globalization;

namespace MyBudget.Pages
{
    public partial class Index
    {
        private BarChartConfig _lastMonthConfig;
        private BarChartConfig _thisMonthConfig;
        private BarChartConfig _lastYearConfig;
        private BarChartConfig _thisYearConfig;

        private int donutIndex = -1;
        private double[] donutChartData;
        private string[] donutChartLabels;
        private Dictionary<string, decimal> donutChartDictionary;

        [Inject] IIncomeAndExpensesViewModelService ViewModelService {get; set;}
        [Inject] IChartProcessor ChartProcessor {get; set;}

		protected override async Task OnInitializedAsync()
        {
            _lastMonthConfig = GetBarCharConfiguration(_lastMonthConfig);
            var lastMonthIncomeExpensesVM = LastMonthIncomeExpenseVM();
            SetBarChartLabelsAndData(_lastMonthConfig, lastMonthIncomeExpensesVM);

            _thisMonthConfig = GetBarCharConfiguration(_thisMonthConfig);
            var thisMonthIncomeExpenseVM = ThisMonthIncomeExpenseVM();
            SetBarChartLabelsAndData(_thisMonthConfig, thisMonthIncomeExpenseVM);

            _lastYearConfig = GetBarCharConfiguration(_lastYearConfig);
            var lastYearIncomeExpenseVM = LastYearIncomeExpenseVM();
            SetBarChartLabelsAndData(_lastYearConfig, lastYearIncomeExpenseVM);

            _thisYearConfig = GetBarCharConfiguration(_thisYearConfig);
            var thisYearIncomeExpenseVM = ThisYearIncomeExpenseVM();
            SetBarChartLabelsAndData(_thisYearConfig, thisYearIncomeExpenseVM);

            donutChartDictionary = await ChartProcessor.DonutChartLastMonthExpenses();
            SetDonutChartData(donutChartDictionary);
        }

        IncomeAndExpensesViewModel LastMonthIncomeExpenseVM()
        {
            IncomeAndExpensesViewModel lastMonthViewModel = new()
            {
                Income = "Last Month Income", 
                IncomeAmount = ViewModelService.LastMonthIncome(), 
                Expenses = "Last Month Expenses", 
                ExpensesAmount = ViewModelService.LastMonthExpenses()
            };
            return lastMonthViewModel;
        }

        IncomeAndExpensesViewModel ThisMonthIncomeExpenseVM()
        {
            IncomeAndExpensesViewModel thisMonthViewModel = new()
            {
                Income = "This Month Income", 
                IncomeAmount = ViewModelService.ThisMonthIncome(), 
                Expenses = "This Month Expenses", 
                ExpensesAmount = ViewModelService.ThisMonthExpenses()
            };
            return thisMonthViewModel;
        }

        IncomeAndExpensesViewModel LastYearIncomeExpenseVM()
        {
            IncomeAndExpensesViewModel lastYearViewModel = new()
            {
                Income = "Last Year Income", 
                IncomeAmount = ViewModelService.LastYearIncome(), 
                Expenses = "Last Year Expenses", 
                ExpensesAmount = ViewModelService.LastYearExpenses()
            };
            return lastYearViewModel;
        }

        IncomeAndExpensesViewModel ThisYearIncomeExpenseVM()
        {
            IncomeAndExpensesViewModel thisYearViewModel = new()
            {
                Income = "This Year Income", 
                IncomeAmount = ViewModelService.ThisYearIncome(), 
                Expenses = "This Year Expenses", 
                ExpensesAmount = ViewModelService.ThisYearExpenses()
            };
            return thisYearViewModel;
        }

        List<string> ViewModelDataLabels(IncomeAndExpensesViewModel viewModel)
        {
            return new List<string>() {viewModel.Income, viewModel.Expenses};
        }

        List<decimal> ViewModelData(IncomeAndExpensesViewModel viewModel)
        {
            return new List<decimal>() {viewModel.IncomeAmount, viewModel.ExpensesAmount};
        }

        BarChartConfig GetBarCharConfiguration(BarChartConfig newBarChart)
        {
			newBarChart = new BarChartConfig()
			{
				Options = new Options()
				{
					Plugins = new Plugins()
					{
						Legend = new Legend()
						{ 
                            Align = LegendAlign.Center, 
                            Display = false, 
                            Position = LegendPosition.Top 
                        }
					},
					Scales = new Dictionary<string, Axis>()
			        {
                        {
                            Scales.XAxisId, new Axis()
			                {
                                Stacked = true, Ticks = new Ticks()
			                    {
                                    MaxRotation = 0, MinRotation = 0
                                }
                            }
                        }, 
                        {
                            Scales.YAxisId, new Axis()
			                {
                                Stacked = true
                            }
                        }
                    }
				}
			};
			return newBarChart;
		}

        void SetBarChartLabelsAndData(BarChartConfig barChartConfig, IncomeAndExpensesViewModel viewModel)
        {
            barChartConfig.Data.Labels = ViewModelDataLabels(viewModel);
            SetBarChartDatasets(barChartConfig, viewModel);
        }

        void SetBarChartDatasets(BarChartConfig barChartConfig, IncomeAndExpensesViewModel viewModel)
        {
			barChartConfig.Data.Datasets.Add(new BarDataset()
			{
				Label = "Value",
				Data = ViewModelData(viewModel),
				BackgroundColor = new List<string>()
			    {
                    "#7aed6d", "#f06772"
                },
                BorderColor = new List<string>()
			    {
                    "green", "red"
                },
				BorderWidth = 1,
				HoverBackgroundColor = new List<string>()
			    {
                    "#15a805", "#b80412"
                }
			});
		}

        private void SetDonutChartData(Dictionary<string, decimal> dictionaryData)
        {
            List<string> chartLabels = new();
            List<double> chartData = new();

            foreach (var keyValuePair in dictionaryData)
            {
				chartLabels.Add($"{keyValuePair.Key} - Total: {keyValuePair.Value.ToString("C2", CultureInfo.CurrentCulture)}");
                chartData.Add(Convert.ToDouble(keyValuePair.Value));
			}

            donutChartLabels = chartLabels.ToArray();
            donutChartData = chartData.ToArray();
        }
    }
}
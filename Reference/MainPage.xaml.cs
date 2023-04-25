using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Reference
{
    
    public partial class MainPage : ContentPage

    {
        private readonly List<double> disabilityRatings = new List<double>();
        public MainPage()
        {
            InitializeComponent();

            ParentsPicker.ItemsSource = new List<int> { 0, 1, 2 };
            ChildrenPicker.ItemsSource = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        private void OnMarriedToggled(object sender, ToggledEventArgs e)
        {
            OnSelectionChanged(sender, e);
        }
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(sender, e);
        }


        private void OnPercentageClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var rating = double.Parse(button.Text.TrimEnd('%'));

            disabilityRatings.Add(rating);
            UpdateEnteredRatingsLabel();

            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);
            CombinedRatingLabel.Text = $"The overall combined disability rating is: {combinedRating}%";
            CompensationLabel.Text = $"{GetAssociatedCompensation(combinedRating)}";
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            disabilityRatings.Clear();
            UpdateEnteredRatingsLabel();
            CombinedRatingLabel.Text = "";
            CompensationLabel.Text = "";
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);
            CombinedRatingLabel.Text = $"The overall combined disability rating is: {combinedRating}%";
            CompensationLabel.Text = $"{GetAssociatedCompensation(combinedRating)}";
        }

        private void UpdateEnteredRatingsLabel()
        {
            EnteredRatingsLabel.Text = "You have selected: " + string.Join(", ", disabilityRatings);
        }

        private double CalculateCombinedDisabilityRating(List<double> disabilityRatings)
        {
            if (disabilityRatings.Count == 0)
            {
                return 0;
            }

            disabilityRatings.Sort((a, b) => b.CompareTo(a));

            double combinedRating = 100;

            foreach (double rating in disabilityRatings)
            {
                combinedRating *= 1 - (rating / 100);
            }

            combinedRating = 100 - combinedRating;
            combinedRating = Math.Round(combinedRating / 10) * 10;

            return combinedRating;
        }

        private List<VACompensationRate> ParseVACompensationRates()
        {
            var compensationRates = new List<VACompensationRate>();

            var rates = new Dictionary<int, Dictionary<(int, int, int), double>>
            {
                {

            10,
            new Dictionary<(int, int, int), double>
            {
                { (0, 0, 0), 165.92 },
                { (0, 0, 1), 165.92 },
                { (0, 1, 0), 165.92 },
                { (0, 1, 1), 165.92 },
                { (0, 2, 0), 165.92 },
                { (0, 2, 1), 165.92 },
                { (1, 0, 0), 165.92 },
                { (1, 0, 1), 165.92 },
                { (1, 1, 0), 165.92 },
                { (1, 1, 1), 165.92 },
                { (1, 2, 0), 165.92 },
                { (1, 2, 1), 165.92 },
            }
        },
        {
            20,
            new Dictionary<(int, int, int), double>
            {
                { (0, 0, 0), 327.99 },
                { (0, 0, 1), 327.99 },
                { (0, 1, 0), 327.99 },
                { (0, 1, 1), 327.99 },
                { (0, 2, 0), 327.99 },
                { (0, 2, 1), 327.99 },
                { (1, 0, 0), 327.99 },
                { (1, 0, 1), 327.99 },
                { (1, 1, 0), 327.99 },
                { (1, 1, 1), 327.99 },
                { (1, 2, 0), 327.99 },
                { (1, 2, 1), 327.99 },
            }
        },
        {
            30,
            new Dictionary<(int, int, int), double>
            {
                { (0, 0, 0), 508.05 },
                { (0, 0, 1), 548.05 },
                { (0, 1, 0), 556.05 },
                { (0, 1, 1), 596.05 },
                { (0, 2, 0), 604.05 },
                { (0, 2, 1), 644.05 },
                { (1, 0, 0), 568.05 },
                { (1, 0, 1), 612.05 },
                { (1, 1, 0), 616.05 },
                { (1, 1, 1), 660.05 },
                { (1, 2, 0), 664.05 },
                { (1, 2, 1), 708.05 },
            }
        },
        {
            40,
            new Dictionary<(int, int, int), double>
            {
                { (0, 0, 0), 731.86 },
                { (0, 0, 1), 785.86 },
                { (0, 1, 0), 795.86 },
                { (0, 1, 1), 849.86 },
                { (0, 2, 0), 859.86 },
                { (0, 2, 1), 913.86 },
                { (1, 0, 0), 811.86 },
                { (1, 0, 1), 870.86 },
                { (1, 1, 0), 875.86 },
                { (1, 1, 1), 934.86 },
                { (1, 2, 0), 939.86 },
                { (1, 2, 1), 998.86 },
            }
        },
        {
            50,
            new Dictionary<(int, int, int), double>
            {
                { (0, 0, 0), 1041.82 },
                { (0, 0, 1), 1108.82 },
                { (0, 1, 0), 1122.82 },
                { (0, 1, 1), 1189.82 },
                { (0, 2, 0), 1203.82 },
                { (0, 2, 1), 1270.82 },
                { (1, 0, 0), 1141.82 },
                { (1, 0, 1), 1215.82 },
                { (1, 1, 0), 1222.82 },
                { (1, 1, 1), 1296.82 },
                { (1, 2, 0), 1303.82 },
                { (1, 2, 1), 1377.82 },
            }
        },
        {
            60,
            new Dictionary<(int, int, int), double>
        {
                { (0, 0, 0), 1319.65 },
                { (0, 0, 1), 1400.65 },
                { (0, 1, 0), 1416.65 },
                { (0, 1, 1), 1497.65 },
                { (0, 2, 0), 1513.65 },
                { (0, 2, 1), 1594.65 },
                { (1, 0, 0), 1440.65 },
                { (1, 0, 1), 1528.65 },
                { (1, 1, 0), 1537.65 },
                { (1, 1, 1), 1625.65 },
                { (1, 2, 0), 1634.65 },
                { (1, 2, 1), 1722.65 },
        }
    },
    {
            70,
            new Dictionary<(int, int, int), double>
        {
                { (0, 0, 0), 1663.06 },
                { (0, 0, 1), 1757.06 },
                { (0, 1, 0), 1776.06 },
                { (0, 1, 1), 1870.06 },
                { (0, 2, 0), 1889.06 },
                { (0, 2, 1), 1983.06 },
                { (1, 0, 0), 1804.06 },
                { (1, 0, 1), 1907.06 },
                { (1, 1, 0), 1917.06 },
                { (1, 1, 1), 2020.06 },
                { (1, 2, 0), 2030.06 },
                { (1, 2, 1), 2133.06 },
        }
    },
    {
            80,
            new Dictionary<(int, int, int), double>
        {
                { (0, 0, 0), 1933.15 },
                { (0, 0, 1), 2041.15 },
                { (0, 1, 0), 2062.15 },
                { (0, 1, 1), 2170.15 },
                { (0, 2, 0), 2191.15 },
                { (0, 2, 1), 2299.15 },
                { (1, 0, 0), 2094.15 },
                { (1, 0, 1), 2212.15 },
                { (1, 1, 0), 2223.15 },
                { (1, 1, 1), 2341.15 },
                { (1, 2, 0), 2353.15 },
                { (1, 2, 1), 2470.15 },
        }
    },
    {
            90,
            new Dictionary<(int, int, int), double>
        {
                { (0, 0, 0), 2172.39 },
                { (0, 0, 1), 2293.39 },
                { (0, 1, 0), 2317.39 },
                { (0, 1, 1), 2438.39 },
                { (0, 2, 0), 2462.39 },
                { (0, 2, 1), 2583.39 },
                { (1, 0, 0), 2353.39 },
                { (1, 0, 1), 2486.39 },
                { (1, 1, 0), 2498.39 },
                { (1, 1, 1), 2631.39 },
                { (1, 2, 0), 2643.39 },
                { (1, 2, 1), 2776.39 },
        }
    },
    {
            100,
            new Dictionary<(int, int, int), double>
        {
                { (0, 0, 0), 3621.95 },
                { (0, 0, 1), 3757.00 },
                { (0, 1, 0), 3784.02 },
                { (0, 1, 1), 3919.07 },
                { (0, 2, 0), 3946.09 },
                { (0, 2, 1), 4081.14 },
                { (1, 0, 0), 3823.89 },
                { (1, 0, 1), 3971.78 },
                { (1, 1, 0), 3985.96 },
                { (1, 1, 1), 4133.85 },
                { (1, 2, 0), 4148.03 },
                { (1, 2, 1), 4295.92 },
        }
    },
};


            foreach (var kvp in rates)
            {
                int disabilityPercentage = kvp.Key;
                var combinationsAndRates = kvp.Value;

                for (int married = 0; married <= 1; married++)
                {
                    for (int parents = 0; parents <= 2; parents++)
                    {
                        for (int children = 0; children <= 9; children++)
                        {
                            if (combinationsAndRates.TryGetValue((married, parents, children), out double rate))
                            {
                                compensationRates.Add(new VACompensationRate
                                {
                                    DisabilityPercentage = disabilityPercentage,
                                    Married = married == 1,
                                    Parents = parents,
                                    Children = children,
                                    Rate = $"${rate:0.00}",
                                });
                            }
                        }
                    }
                }
            }

            return compensationRates;
        }
        private string GetAssociatedCompensation(double combinedRating)
        {
            int roundedCombinedRating = (int)Math.Round(combinedRating);
            bool isMarried = MarriedSwitch.IsToggled;
            int parents = ParentsPicker.SelectedIndex;
            int children = ChildrenPicker.SelectedIndex;

            var rates = ParseVACompensationRates();


            var closestRate = rates.OrderBy(rate => Math.Abs(rate.DisabilityPercentage - roundedCombinedRating))
                                    .ThenBy(rate => Math.Abs((rate.Married ? 1 : 0) - (isMarried ? 1 : 0)))
                                    .ThenBy(rate => Math.Abs(rate.Parents - parents))
                                    .ThenBy(rate => Math.Abs(rate.Children - children))
                                    .FirstOrDefault();

            return closestRate?.Rate ?? "N/A";
        }

    }
}
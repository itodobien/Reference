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

        private string GetAssociatedCompensation(double combinedRating)
        {
            int roundedCombinedRating = (int)Math.Round(combinedRating);
            bool isMarried = MarriedSwitch.IsToggled;
            int parents = ParentsPicker.SelectedIndex;
            int children = ChildrenPicker.SelectedIndex;

            var rates = VACompensationRateParser.ParseVACompensationRates();

            var closestRate = rates.OrderBy(rate => Math.Abs(rate.DisabilityPercentage - roundedCombinedRating))
                                    .ThenBy(rate => Math.Abs((rate.Married ? 1 : 0) - (isMarried ? 1 : 0)))
                                    .ThenBy(rate => Math.Abs(rate.Parents - parents))
                                    .ThenBy(rate => Math.Abs(rate.Children - children))
                                    .FirstOrDefault();

            return closestRate?.Rate ?? "N/A";
        }
    }
}

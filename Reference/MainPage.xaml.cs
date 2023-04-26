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
            Button button = sender as Button;
            int disabilityPercentage = int.Parse(button.Text.TrimEnd('%'));
            disabilityRatings.Add(disabilityPercentage);
            UpdateEnteredRatingsLabel();

            int numChildrenUnder18 = ChildrenPicker.SelectedItem != null ? (int)ChildrenPicker.SelectedItem : 0;
            int numChildrenOver18InSchool = ChildrenPicker18.SelectedItem != null ? (int)ChildrenPicker18.SelectedItem : 0;

            // Calculate the combined rating first
            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);

            double compensationAmount = CalculateCompensation((int)combinedRating, numChildrenUnder18, numChildrenOver18InSchool);

            EnteredRatingsLabel.Text = $"You have selected: {disabilityPercentage}%";
            CompensationLabel.Text = $" ${compensationAmount:0.00}";

            OnSelectionChanged(sender, e);
        }


        private double CalculateCompensation(int disabilityPercentage, int numChildrenUnder18, int numChildrenOver18InSchool)
        {
            double compensationAmount = 0;
            int roundedCombinedRating = (int)Math.Round((double)disabilityPercentage);
            bool isMarried = MarriedSwitch.IsToggled;
            int parents = ParentsPicker.SelectedIndex;
            int children = ChildrenPicker.SelectedIndex;

            var rates = VACompensationRateParser.ParseVACompensationRates();

            var closestRate = rates.OrderBy(rate => Math.Abs(rate.DisabilityPercentage - roundedCombinedRating))
                                    .ThenBy(rate => Math.Abs((rate.Married ? 1 : 0) - (isMarried ? 1 : 0)))
                                    .ThenBy(rate => Math.Abs(rate.Parents - parents))
                                    .ThenBy(rate => Math.Abs(rate.Children - children))
                                    .FirstOrDefault();

            if (closestRate != null && double.TryParse(closestRate.Rate, out double rateValue))
            {
                compensationAmount += rateValue;
            }

            if (disabilityPercentage >= 30)
            {
                int childBonusUnder18 = 30;
                int childBonusOver18InSchool = 97;

                if (numChildrenUnder18 > 1)
                {
                    compensationAmount += (numChildrenUnder18 - 1) * childBonusUnder18;
                }

                if (numChildrenOver18InSchool > 1)
                {
                    compensationAmount += (numChildrenOver18InSchool - 1) * childBonusOver18InSchool;
                }
            }

            return compensationAmount;
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

            int numChildrenUnder18 = ChildrenPicker.SelectedItem != null ? (int)ChildrenPicker.SelectedItem : 0;
            int numChildrenOver18InSchool = ChildrenPicker18.SelectedItem != null ? (int)ChildrenPicker18.SelectedItem : 0;

            double compensationAmount = CalculateCompensation((int)combinedRating, numChildrenUnder18, numChildrenOver18InSchool);
            CompensationLabel.Text = $" ${compensationAmount}";
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
    }
}
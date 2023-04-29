using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.CommunityToolkit;

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
            ChildrenPicker18.ItemsSource = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
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

            int numChildrenUnder18 = ChildrenPicker.SelectedItem != null ? (int)ChildrenPicker.SelectedItem : 0;
            int numChildrenOver18InSchool = ChildrenPicker18.SelectedItem != null ? (int)ChildrenPicker18.SelectedItem : 0;

            // Calculate the combined rating first
            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);

            double compensationAmount = CalculateCompensation((int)combinedRating, numChildrenUnder18, numChildrenOver18InSchool);

            // Add the selected percentage to the EnteredRatingsLabel
            if (EnteredRatingsLabel.Text == "You have selected: ")
            {
                EnteredRatingsLabel.Text += $"{disabilityPercentage}%";
            }
            else
            {
                EnteredRatingsLabel.Text += $", {disabilityPercentage}%";
            }

            CompensationLabel.Text = "$ " + compensationAmount.ToString("F2");

            OnSelectionChanged(sender, e);
        }

            //Calculation for overall compensation. Pulls from VACompensationRate.cs dictionary then adds appropriate compensation for any additional children.
        private double CalculateCompensation(int disabilityPercentage, int numChildrenUnder18, int numChildrenOver18InSchool) //VA allows different amounts depending on age.
        {
            double compensationAmount = 0;
            int roundedCombinedRating = (int)Math.Round((double)disabilityPercentage);
            bool isMarried = MarriedSwitch.IsToggled;
            int parents = ParentsPicker.SelectedIndex != -1 ? ParentsPicker.SelectedIndex : 0;

            var rates = VACompensationRateParser.GetParsedRates(); //Fetches dictionary of rates from VACompensationRate.cs

            if (rates.TryGetValue(roundedCombinedRating, out var rateDictionary)) //Checks if the dictionary contains the rounded combined rating
            {
                int childrenToConsider = Math.Min(numChildrenUnder18 + numChildrenOver18InSchool, 1); // consider at most 1 child for the rate dictionary

                if (rateDictionary.TryGetValue((isMarried ? 1 : 0, parents, childrenToConsider), out string rateValueStr))
                {
                    if (double.TryParse(rateValueStr.Replace("$", ""), out double rateValue))
                    {
                        compensationAmount += rateValue; //
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to parse rate value: {rateValueStr}");
                        System.Diagnostics.Debug.WriteLine($"No rate found for given parameters. Marital status: {isMarried}, Parents: {parents}, Children: {numChildrenUnder18},{numChildrenOver18InSchool}");
                        System.Diagnostics.Debug.WriteLine($"No rates found for given disability percentage: {roundedCombinedRating}");
                    }
                }
            }

            // Calculate the child bonus separately if there are more than one child
            double childBonus = 0;

            // Only calculate child bonus if the disability percentage is =>30
            if (roundedCombinedRating == 30)
            {
                // Calculate bonus for children under 18
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 30; // subtract 1 to not count the first child twice
                }

                // Calculate bonus for children over 18 in school
                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 97; 
                }

                // Add an extra $97 if there is at least one child in each category, but not for the first child
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 97;
                }

                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 40)
            {
                
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 40; 
                }

                
                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 129; 
                }

                
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 129;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 50)
            {

                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 50;
                }
                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 162; 
                }
                
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 162;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 60)
            {               
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 60; 
                }

                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 194; 
                }

                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 194;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 70)
            {               
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 70; 
                }

                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 226; 
                }
               
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 226;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 80)
            {
                
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 80; 
                }

                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 259; 
                }
                
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 259;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 90)
            {
                
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 90; 
                }

                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 291; 
                }
               
                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 291;
                }
                compensationAmount += childBonus;
            }
            else if (roundedCombinedRating == 100)
            {
                
                if (numChildrenUnder18 > 1)
                {
                    childBonus += (numChildrenUnder18 - 1) * 100; 
                }

               
                if (numChildrenOver18InSchool > 1)
                {
                    childBonus += (numChildrenOver18InSchool - 1) * 324.12; 
                }

                if (numChildrenUnder18 > 0 && numChildrenOver18InSchool > 0)
                {
                    childBonus += 324.12;
                }

                compensationAmount += childBonus;
            }
            return compensationAmount;
        }


        private void OnChildrenCountChanged(object sender, EventArgs e)
        {
            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);
            CombinedRatingLabel.Text = $"The overall combined disability rating is: {combinedRating}%";

            int numChildrenUnder18 = ChildrenPicker.SelectedItem != null ? (int)ChildrenPicker.SelectedItem : 0;
            int numChildrenOver18InSchool = ChildrenPicker18.SelectedItem != null ? (int)ChildrenPicker18.SelectedItem : 0;

            double compensationAmount = CalculateCompensation((int)combinedRating, numChildrenUnder18, numChildrenOver18InSchool);
            CompensationLabel.Text = $"$ {compensationAmount}";
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
            CompensationLabel.Text = $"$ {compensationAmount}";
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
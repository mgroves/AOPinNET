using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CachingPostSharp
{
    public partial class CarValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getValueButton.Click += getValueButton_Click;

            if (!IsPostBack)
                PopulateDropDowns();
        }

        void PopulateDropDowns()
        {
            makeDropDown.Items.AddRange(new[] {
                new ListItem("Honda", "0"),
                new ListItem("Toyota", "1"),
                new ListItem("Ford", "2")
            });
            yearDropDown.Items.AddRange(new[] {
                new ListItem("2010"), 
                new ListItem("2011"),
                new ListItem("2012")
            });
            conditionDropDown.Items.AddRange(new[] {
                new ListItem("Poor", "0"),
                new ListItem("Average", "1"),
                new ListItem("Mint", "2")
            });
        }

        protected override void OnPreRender(EventArgs e)
        {
            DisplayCache();
        }

        void DisplayCache()
        {
            cachedItemsList.Items.Clear();
            foreach (DictionaryEntry cachedItem in Cache)
            {
                var cacheRecord = cachedItem.Key + " - " + cachedItem.Value;
                if (cacheRecord.Contains("__System.Web.WebPages.Deployment__"))
                    continue;
                cachedItemsList.Items.Add(new ListItem(cacheRecord));
            }
            if(cachedItemsList.Items.Count == 0)
                cachedItemsList.Items.Add(new ListItem("None"));
        }

        void getValueButton_Click(object sender, EventArgs e)
        {
            var makeId = int.Parse(makeDropDown.SelectedValue);
            var year = int.Parse(yearDropDown.SelectedValue);
            var conditionId = int.Parse(conditionDropDown.SelectedValue);

            var args = new CarValueArgs
            {
                MakeId = int.Parse(makeDropDown.SelectedValue),
                Year = int.Parse(yearDropDown.SelectedValue),
                ConditionId = int.Parse(conditionDropDown.SelectedValue)
            };

            var carValueService = new CarValueService();
            //var carValue = carValueService.GetValue(makeId, conditionId, year);
            var carValue = carValueService.GetValue(args);
            valueLiteral.Text = carValue.ToString("c");
        }
    }
}

using Grigorovici_Tonia_Lab7.Models;
namespace Grigorovici_Tonia_Lab7
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;
            await App.Database.SaveShopListAsync(slist);
            await Navigation.PopAsync();
        }
        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            await App.Database.DeleteShopListAsync(slist);
            await Navigation.PopAsync();
        }

        async void OnDeleteItemButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            if (listView.SelectedItem != null)
            {
                var selectedProduct = (Product)listView.SelectedItem;
                var allProducts = await App.Database.GetAllProducts();
                var productToDelete = allProducts.Find(x => x.ProductID == selectedProduct.ID && x.ShopListID == slist.ID);
                if(productToDelete != null)
                {
                    await App.Database.DeleteListProductAsync(productToDelete);
                    await Navigation.PopAsync();
                }
             
            }
        }

        async void OnChooseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage((ShopList)
            this.BindingContext)
            {
                BindingContext = new Product()
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var shopl = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
    }
}
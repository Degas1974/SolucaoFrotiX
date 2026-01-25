namespace FrotiX.Mobile.Economildo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Autenticação será tratada dentro do Blazor
        }
    }
}

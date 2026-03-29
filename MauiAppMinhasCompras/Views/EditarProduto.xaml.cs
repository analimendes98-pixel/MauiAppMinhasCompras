using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // isso aqui vai fazer o picker mostrar a categoria que o produto já tinha
        if (BindingContext is Produto produto_anexado)
        {
            pck_categoria.SelectedItem = produto_anexado.Categoria;
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // pega o objeto que tá "pendurado" na página 
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id, // vai manter o id original para atualizar o registro certo
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),

                
                Categoria = pck_categoria.SelectedItem?.ToString()
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
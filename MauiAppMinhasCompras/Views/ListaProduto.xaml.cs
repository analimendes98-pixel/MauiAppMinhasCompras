using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            lst_produtos.IsRefreshing = true;
            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // filtro por categoria
    private async void pck_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string selecionado = pck_filtro_categoria.SelectedItem.ToString();
            List<Produto> todos = await App.Db.GetAll();
            lista.Clear();

            if (selecionado == "Todos")
            {
                todos.ForEach(i => lista.Add(i));
            }
            else
            {
                var filtrados = todos.Where(p => p.Categoria == selecionado).ToList();
                filtrados.ForEach(i => lista.Add(i));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // relatorio de gastos por categoria
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            if (lista.Count == 0)
            {
                DisplayAlert("Atenção", "Lista vazia.", "OK");
                return;
            }

            var relatorio = lista.GroupBy(p => p.Categoria)
                                 .Select(g => $"{g.Key}: {g.Sum(p => p.Total):C}")
                                 .ToList();

            string msg = string.Join("\n", relatorio);

            DisplayAlert("Gastos por Categoria", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecinado = sender as MenuItem;
            Produto p = selecinado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            if (p != null)
            {
                Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}
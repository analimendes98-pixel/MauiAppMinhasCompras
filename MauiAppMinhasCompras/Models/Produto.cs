using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        string _categoria; 

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
                _descricao = value;
            }
        }

        // tá aqui o novo campo categoria :)
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, selecione uma categoria");
                }
                _categoria = value;
            }
        }
       

        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }
    }
}
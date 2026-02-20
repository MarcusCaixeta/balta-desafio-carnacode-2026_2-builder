using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{

    public class SalesReport
    {
        public string Title { get; }
        public string Format { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public bool IncludeHeader { get; }
        public string HeaderText { get; }
        public bool IncludeFooter { get; }
        public string FooterText { get; }
        public bool IncludeCharts { get; }
        public string ChartType { get; }
        public bool IncludeSummary { get; }
        public IReadOnlyList<string> Columns { get; }
        public IReadOnlyList<string> Filters { get; }
        public string SortBy { get; }
        public string GroupBy { get; }
        public bool IncludeTotals { get; }
        public string Orientation { get; }
        public string PageSize { get; }
        public bool IncludePageNumbers { get; }
        public string CompanyLogo { get; }
        public string WaterMark { get; }

        // Construtor interno - só o Builder pode criar
        internal SalesReport(SalesReportBuilder builder)
        {
            Title = builder.Title;
            Format = builder.Format;
            StartDate = builder.StartDate;
            EndDate = builder.EndDate;
            IncludeHeader = builder.IncludeHeader;
            HeaderText = builder.HeaderText;
            IncludeFooter = builder.IncludeFooter;
            FooterText = builder.FooterText;
            IncludeCharts = builder.IncludeCharts;
            ChartType = builder.ChartType;
            IncludeSummary = builder.IncludeSummary;
            Columns = builder.Columns.AsReadOnly();
            Filters = builder.Filters.AsReadOnly();
            SortBy = builder.SortBy;
            GroupBy = builder.GroupBy;
            IncludeTotals = builder.IncludeTotals;
            Orientation = builder.Orientation;
            PageSize = builder.PageSize;
            IncludePageNumbers = builder.IncludePageNumbers;
            CompanyLogo = builder.CompanyLogo;
            WaterMark = builder.WaterMark;
        }

        public void Generate()
        {
            Console.WriteLine($"\n=== Gerando Relatório: {Title} ===");
            Console.WriteLine($"Formato: {Format}");
            Console.WriteLine($"Período: {StartDate:dd/MM/yyyy} a {EndDate:dd/MM/yyyy}");

            if (IncludeHeader)
                Console.WriteLine($"Cabeçalho: {HeaderText}");

            Console.WriteLine($"Colunas: {string.Join(", ", Columns)}");

            if (Filters.Count > 0)
                Console.WriteLine($"Filtros: {string.Join(", ", Filters)}");

            if (!string.IsNullOrEmpty(GroupBy))
                Console.WriteLine($"Agrupado por: {GroupBy}");

            if (IncludeCharts)
                Console.WriteLine($"Gráfico: {ChartType}");

            if (IncludeTotals)
                Console.WriteLine("Inclui totais");

            if (IncludeFooter)
                Console.WriteLine($"Rodapé: {FooterText}");

            Console.WriteLine("Relatório gerado com sucesso!");
        }
    }
    
    public class SalesReportBuilder
    {
        // Obrigatórios
        internal string Title { get; private set; }
        internal string Format { get; private set; }
        internal DateTime StartDate { get; private set; }
        internal DateTime EndDate { get; private set; }

        // Opcionais
        internal bool IncludeHeader;
        internal string HeaderText;
        internal bool IncludeFooter;
        internal string FooterText;
        internal bool IncludeCharts;
        internal string ChartType;
        internal bool IncludeSummary;
        internal List<string> Columns = new();
        internal List<string> Filters = new();
        internal string SortBy;
        internal string GroupBy;
        internal bool IncludeTotals;
        internal string Orientation = "Portrait";
        internal string PageSize = "A4";
        internal bool IncludePageNumbers;
        internal string CompanyLogo;
        internal string WaterMark;

        // Construtor exige campos obrigatórios
        public SalesReportBuilder(string title, string format, DateTime start, DateTime end)
        {
            Title = title;
            Format = format;
            StartDate = start;
            EndDate = end;
        }

        public SalesReportBuilder AddColumn(string column)
        {
            Columns.Add(column);
            return this;
        }

        public SalesReportBuilder AddFilter(string filter)
        {
            Filters.Add(filter);
            return this;
        }

        public SalesReportBuilder WithHeader(string text)
        {
            IncludeHeader = true;
            HeaderText = text;
            return this;
        }

        public SalesReportBuilder WithFooter(string text)
        {
            IncludeFooter = true;
            FooterText = text;
            return this;
        }

        public SalesReportBuilder WithChart(string chartType)
        {
            IncludeCharts = true;
            ChartType = chartType;
            return this;
        }

        public SalesReportBuilder GroupByField(string groupBy)
        {
            GroupBy = groupBy;
            return this;
        }

        public SalesReportBuilder WithTotals()
        {
            IncludeTotals = true;
            return this;
        }

        public SalesReportBuilder Landscape()
        {
            Orientation = "Landscape";
            return this;
        }

        public SalesReportBuilder WithPageNumbers()
        {
            IncludePageNumbers = true;
            return this;
        }

        public SalesReport Build()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new InvalidOperationException("Título é obrigatório.");

            if (StartDate > EndDate)
                throw new InvalidOperationException("Data inicial não pode ser maior que a final.");

            if (Columns.Count == 0)
                throw new InvalidOperationException("O relatório deve ter pelo menos uma coluna.");

            return new SalesReport(this);
        }
    }
   
    public static class ReportPresets
    {
        public static SalesReportBuilder DefaultSales(
            string title,
            DateTime start,
            DateTime end)
        {
            return new SalesReportBuilder(title, "PDF", start, end)
                .AddColumn("Produto")
                .AddColumn("Quantidade")
                .AddColumn("Valor")
                .WithHeader("Relatório de Vendas")
                .WithFooter("Confidencial")
                .WithTotals();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Relatórios (Builder Pattern) ===");

            var report1 = new SalesReportBuilder(
                    "Vendas Mensais",
                    "PDF",
                    new DateTime(2024, 1, 1),
                    new DateTime(2024, 1, 31))
                .AddColumn("Produto")
                .AddColumn("Quantidade")
                .AddColumn("Valor")
                .AddFilter("Status=Ativo")
                .WithHeader("Relatório de Vendas")
                .WithFooter("Confidencial")
                .WithChart("Bar")
                .GroupByField("Categoria")
                .WithTotals()
                .Landscape()
                .WithPageNumbers()
                .Build();

            report1.Generate();


            var report2 = ReportPresets
                .DefaultSales(
                    "Vendas Anuais",
                    new DateTime(2024, 1, 1),
                    new DateTime(2024, 12, 31))
                .WithChart("Pie")
                .Landscape()
                .Build();

            report2.Generate();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;
using System.Collections.ObjectModel;

namespace QuanLyDaiLy.Views.AgencyList;

public partial class Agency : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataContext _context;
    public ObservableCollection<DaiLy>? DaiLyList { get; set; }

    public Agency(DataContext context, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _context = context;
        _serviceProvider = serviceProvider;

        LoadData();
        BindingContext = this;
    }
    private void LoadData()
    {
        var ds = _context.DsDaiLy
                         .Include(d => d.Quan)
                         .Include(d => d.LoaiDaiLy)
                         .ToList();

        DaiLyList = new ObservableCollection<DaiLy>(ds);
    }
    private async void OnAddAgencyClicked(object sender, EventArgs e)
    {
        var page = _serviceProvider.GetRequiredService<QuanLyDaiLy.Views.AgencyAdd.AgencyAdd>();
        await Navigation.PushAsync(page);
    }
}

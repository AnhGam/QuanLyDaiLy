using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Data;
using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Views.AgencyAdd
{
    public partial class AgencyAdd : ContentPage
    {
        private readonly DataContext _context;

        public AgencyAdd(DataContext context)
        {
            InitializeComponent();
            _context = context;

            // Default date for Datepicker
            dateNgayTiepNhan.Date = DateTime.Today;

            // Load dữ liệu cho Picker
            LoadPickers();
        }

        private void LoadPickers()
        {
            // Load Quận
            var dsQuan = _context.DsQuan.ToList();
            pickerQuan.ItemsSource = dsQuan;
            pickerQuan.ItemDisplayBinding = new Binding("TenQuan");

            // Load Loại đại lý
            var dsLoai = _context.DsLoaiDaiLy.ToList();
            pickerLoaiDaiLy.ItemsSource = dsLoai;
            pickerLoaiDaiLy.ItemDisplayBinding = new Binding("TenLoaiDaiLy");
        }

        private async void OnSaveAgencyClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDaiLy.Text))
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập tên đại lý!", "OK");
                return;
            }

            if (pickerQuan.SelectedItem == null || pickerLoaiDaiLy.SelectedItem == null)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn Quận và Loại đại lý!", "OK");
                return;
            }

            var selectedQuan = (Quan)pickerQuan.SelectedItem;
            var selectedLoai = (LoaiDaiLy)pickerLoaiDaiLy.SelectedItem;

            var newDaiLy = new DaiLy
            {
                TenDaiLy = txtTenDaiLy.Text,
                DienThoai = txtDienThoai.Text,
                Email = txtEmail.Text,
                DiaChi = txtDiaChi.Text,
                NgayTiepNhan = dateNgayTiepNhan.Date,
                MaQuan = selectedQuan.MaQuan,
                MaLoaiDaiLy = selectedLoai.MaLoaiDaiLy
            };

            _context.DsDaiLy.Add(newDaiLy);
            await _context.SaveChangesAsync();

            await DisplayAlert("Thông báo", "Lưu thành công!", "OK");
            await Navigation.PopAsync(); // Quay về trang trước
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            txtTenDaiLy.Text = string.Empty;
            txtDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            if (pickerQuan.ItemsSource != null && pickerQuan.ItemsSource.Count > 0)
                pickerQuan.SelectedIndex = 0;
            if (pickerLoaiDaiLy.ItemsSource != null && pickerLoaiDaiLy.ItemsSource.Count > 0)
                pickerLoaiDaiLy.SelectedIndex = 0;
            dateNgayTiepNhan.Date = DateTime.Today;
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

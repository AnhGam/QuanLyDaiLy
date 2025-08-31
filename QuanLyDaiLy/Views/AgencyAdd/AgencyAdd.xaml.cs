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

            // Gắn sự kiện để preview Mã đại lý
            txtTenDaiLy.TextChanged += (s, e) => PreviewMaDaiLy();
            txtDienThoai.TextChanged += (s, e) => PreviewMaDaiLy();
            txtEmail.TextChanged += (s, e) => PreviewMaDaiLy();
            txtDiaChi.TextChanged += (s, e) => PreviewMaDaiLy();
            pickerQuan.SelectedIndexChanged += (s, e) => PreviewMaDaiLy();
            pickerLoaiDaiLy.SelectedIndexChanged += (s, e) => PreviewMaDaiLy();
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

        private void PreviewMaDaiLy()
        {
            // Chỉ preview khi đã nhập đủ thông tin cần thiết
            if (string.IsNullOrWhiteSpace(txtTenDaiLy.Text) ||
                string.IsNullOrWhiteSpace(txtDienThoai.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                pickerQuan.SelectedItem == null ||
                pickerLoaiDaiLy.SelectedItem == null)
            {
                txtMaDaiLy.Text = string.Empty;
                txtMaDaiLy.BackgroundColor = Colors.LightGray;
                txtMaDaiLy.TextColor = Colors.Black;
                return;
            }

            string ten = txtTenDaiLy.Text.Trim();
            var exists = _context.DsDaiLy.Any(d => d.TenDaiLy == ten);

            if (exists)
            {
                txtMaDaiLy.Text = "Đã tồn tại!";
                txtMaDaiLy.TextColor = Colors.Red;
            }
            else
            {
                int nextId = 1;
                if (_context.DsDaiLy.Any())
                    nextId = _context.DsDaiLy.Max(d => d.MaDaiLy) + 1;

                txtMaDaiLy.Text = nextId.ToString();
                txtMaDaiLy.TextColor = Colors.Black;
            }
        }

        private async void OnSaveAgencyClicked(object sender, EventArgs e)
        {
            if (txtMaDaiLy.Text == "(Tự động sinh)" || txtMaDaiLy.Text == "Đã tồn tại!")
            {
                await DisplayAlert("Lỗi", "Dữ liệu chưa hợp lệ!", "OK");
                return;
            }

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

            await DisplayAlert("Thông báo", $"Lưu thành công! Mã đại lý: {newDaiLy.MaDaiLy}", "OK");        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            txtTenDaiLy.Text = string.Empty;
            txtDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtMaDaiLy.Text = "(Tự động sinh)";
            txtMaDaiLy.TextColor = Colors.Gray;

            if (pickerQuan.ItemsSource != null && pickerQuan.ItemsSource.Count > 0)
                pickerQuan.SelectedIndex = -1;
            if (pickerLoaiDaiLy.ItemsSource != null && pickerLoaiDaiLy.ItemsSource.Count > 0)
                pickerLoaiDaiLy.SelectedIndex = -1;

            dateNgayTiepNhan.Date = DateTime.Today;
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

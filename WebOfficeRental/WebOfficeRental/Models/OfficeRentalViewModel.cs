using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebOfficeRental.Models
{
    public class MemuVM
    {
        public int menu_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên menu")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        [Display(Name = "Tên Menu")]
        public string menu_name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ truy cập")]
        [StringLength(505, ErrorMessage = "{0} không được dài quá 505 ký tự.")]
        [Display(Name = "Link menu")]
        public string menu_url { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn menu cha.")]
        [Display(Name = "Menu cha")]
        public int? menu_parent_id { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn vị trí menu.")]
        [Display(Name = "Vị trí menu")]
        public int? menu_position { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thứ tự menu.")]
        [Display(Name = "Thứ tự menu")]
        public int? menu_position_index { get; set; }
    }

    public class LstMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }
        public int? MenuPositionIndex { get; set; }
        public IList<LstMenu> LstMenus { get; set; }
        public LstMenu()
        {
            LstMenus = new List<LstMenu>();
        }
    }

    // categorys class 
    public class CatVM
    {
        public int art_cat_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(500, ErrorMessage = "{0} không được dài quá 500 ký tự.")]
        [Display(Name = "Tên danh mục")]
        public string art_cat_name { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục cha")]
        [Display(Name = "Danh mục cha")]
        public int? art_cat_parent_id { get; set; }
    }

    public class LstCat
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int? ParentCatId { get; set; }
       
        public IList<LstCat> LstCats { get; set; }
        public LstCat()
        {
            LstCats = new List<LstCat>();
        }
    }

    public class CityVM
    {
        public int city_id { get; set; }
        [Display(Name = "Tên quận huyện/thành phố")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string district { get; set; }
        [Display(Name = "Tỉnh thành")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string provinces { get; set; }
    }

    public class ServerOfficeVM
    {
        public int Id { get; set; }
        [Display(Name = "Dịch vụ văn phòng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(250, ErrorMessage = "{0} không được dài quá 250 ký tự.")]
        public string name { get; set; }
    }

    public class BuildsVM
    {
        public int bulding_id { get; set; }
        [Display(Name = "Tên tòa nhà")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string bulding_name { get; set; }
        public int? city_id { get; set; }
        [Display(Name = "Quận/huyện")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string district { get; set; }
        [Display(Name = "Tỉnh thành")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string provinces { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string building_address { get; set; }
        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string building_picture { get; set; }
        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(50, ErrorMessage = "{0} không được dài quá 50 ký tự.")]
        public string building_phonenumber { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="{0} Không đúng định dạng, phải là example@gmail.com")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(250, ErrorMessage = "{0} không được dài quá 250 ký tự.")]
        public string building_email { get; set; }
        [Display(Name = "Fanpage")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string building_fanpage { get; set; }
        public string building_latlong { get; set; }
        public string building_description { get; set; }
        public string building_services { get; set; }
    }

    public class OfficeVM
    {
        public long office_id { get; set; }

        [Display(Name = "Tên văn phòng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} không được dài quá 255 ký tự.")]
        public string office_name { get; set; }

        [Display(Name = "Loại phòng cho thuê")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public Nullable<int> office_type { get; set; }

        [Display(Name = "Loại tin đăng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public Nullable<int> office_new_type { get; set; }

        [Display(Name = "Địa chỉ văn phòng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(505, ErrorMessage = "{0} không được dài quá 505 ký tự.")]
        public string office_address { get; set; }

        [Display(Name = "Giá thuê")]
        [Required(ErrorMessage = "Nhập giá cho thuê phòng")]
        public Nullable<int> office_price_public { get; set; }

        [Display(Name = "Địa chỉ Email")]
        //[Required(ErrorMessage = "{0} không được để trống.")]
        //[StringLength(50, ErrorMessage = "{0} không được dài quá 50 ký tự.")]
        [EmailAddress(ErrorMessage="Email không đúng định dạng")]
        public string office_hotmail { get; set; }

        [Display(Name = "Số hotline")]
        //[Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(12, ErrorMessage = "{0} không được dài quá 12 ký tự.")]
        public string office_hotline { get; set; }

        [Display(Name = "Fanpage văn phòng")]
        //[Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(505, ErrorMessage = "{0} không được dài quá 505 ký tự.")]
        public string office_fanpage { get; set; }

        [Display(Name = "Diện tích văn phòng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public Nullable<int> office_acreage { get; set; }

        //[Display(Name = "Số cửa")]
        ////[Required(ErrorMessage = "{0} không được để trống.")]
        //public Nullable<int> office_door { get; set; }

        //[Display(Name = "Số bàn ghế")]
        ////[Required(ErrorMessage = "{0} không được để trống.")]
        //public Nullable<int> office_table { get; set; }

        [Display(Name = "Ảnh văn phòng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(250, ErrorMessage = "{0} không được dài quá 250 ký tự.")]
        public string office_photo { get; set; }

        [Display(Name = "Ảnh lớn")]
        public string office_photo_slider { get; set; }

        [Display(Name = "Thông tin thêm")]
        public string office_other_descriptions { get; set; }

        [Display(Name = "Tòa nhà")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public Nullable<int> buiding_id { get; set; }

        public int[] dichvuvp { get; set; }

        public bool? status { get; set; }
        public string donvi { get; set; }
    }

    public class articlesVM
    {
        public long article_id { get; set; }
        public string tags { get; set; }
        [Display(Name = "Danh mục bài viết")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public int? art_cat_id { get; set; }
        [Display(Name = "Tên bài viết")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(500, ErrorMessage = "{0} không được dài quá 500 ký tự.")]
        public string article_name { get; set; }
        [Display(Name = "Mô tả bài viết")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(500, ErrorMessage = "{0} không được dài quá 500 ký tự.")]
        public string article_description { get; set; }
        [Display(Name = "Nội dung bài viết")]        
        public string article_content { get; set; }
        [Display(Name = "Kiểu bài viết")]
        public int? article_type { get; set; }
        [Display(Name = "Đường dẫn truy cập")]
        public string article_slugurl { get; set; }
        [Display(Name = "Hình ảnh bài viết (ảnh nhỏ)")]
        public string article_photo_sm { get; set; }
        [Display(Name = "Hình ảnh bài viết (ảnh lớn)")]
        public string article_photo_lg { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }

    }

    public class banerVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên banner")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(250, ErrorMessage = "{0} không được dài quá 250 ký tự.")]
        public string name { get; set; }
        [Display(Name = "Đường dẫn")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        public string link { get; set; }
        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(500, ErrorMessage = "{0} không được dài quá 500 ký tự.")]
        public string photo { get; set; }
    }


}
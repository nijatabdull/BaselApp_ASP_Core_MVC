using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.PageModel.AccountPageModel;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using BaselFinalProjectApp.Models.PageModel.ShopPageModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Data
{
    public class BaselDbContext : IdentityDbContext<AppUser>
    {
        public BaselDbContext(DbContextOptions<BaselDbContext> identityOptions) : base(identityOptions) { }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<BillingDetail> BillingDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<CompareList> CompareLists { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<SocialAccount> SocialAccounts { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<AccountMenu> AccountMenus { get; set; }
        public virtual DbSet<NotFound> NotFounds { get; set; }
        public virtual DbSet<AddtionalMenu> AddtionalMenus { get; set; }
        public virtual DbSet<CompanyLogo> CompanyLogos { get; set; }
        public virtual DbSet<ContactAbout> ContactAbouts { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<FeaturedProduct> FeaturedProducts { get; set; }
        public virtual DbSet<HeadMenu> HeadMenus { get; set; }
        public virtual DbSet<HomeConnect> HomeConnects { get; set; }
        public virtual DbSet<HomeSocialAccount> HomeSocialAccounts { get; set; }
        public virtual DbSet<LatestNews> LatesNews { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<PageAbout> PageAbouts { get; set; }
        public virtual DbSet<PageLogo> PageLogos { get; set; }
        public virtual DbSet<PageSocialAccount> PageSocialAccounts { get; set; }
        public virtual DbSet<PersonAbout> PersonAbouts { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<SubMenu> SubMenus { get; set; }
        public virtual DbSet<FilterColor> FilterColors { get; set; }
        public virtual DbSet<FilterPrice> FilterPrices { get; set; }
        public virtual DbSet<FilterSize> FilterSizes { get; set; }
        public virtual DbSet<FilterSortBy> FilterSortBies { get; set; }
        public virtual DbSet<ShopMenu> ShopMenus { get; set; }
        public virtual DbSet<ShoppingDelivery> ShoppingDeliveries { get; set; }
        public DbSet<BaselFinalProjectApp.Models.PageModel.HomePageModel.HeadMenuLanguage> HeadMenuLanguage { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //ProductCart
            builder.Entity<ProductCart>()
                .HasKey(k=> new { k.CartId,k.ProductId });

            builder.Entity<ProductCart>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductCarts)
                        .HasForeignKey(p=>p.ProductId);

            builder.Entity<ProductCart>()
                .HasOne(c => c.Cart)
                    .WithMany(pc => pc.ProductCarts)
                        .HasForeignKey(c=>c.CartId);

            //ProductColor
            builder.Entity<ProductColor>()
                .HasKey(k=> new { k.ProductId,k.ColorId });

            builder.Entity<ProductColor>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductColors)
                        .HasForeignKey(c=>c.ProductId);

            builder.Entity<ProductColor>()
                .HasOne(c => c.Color)
                    .WithMany(pc => pc.ProductColors)
                        .HasForeignKey(c=>c.ColorId);

            //ProductWishList
            builder.Entity<ProductWishList>()
                .HasKey(k => new { k.ProductId, k.WishListId });

            builder.Entity<ProductWishList>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductWishLists)
                        .HasForeignKey(c => c.ProductId);

            builder.Entity<ProductWishList>()
                .HasOne(c => c.WishList)
                    .WithMany(pc => pc.ProductWishLists)
                        .HasForeignKey(c => c.WishListId);

            //ProductCompareList
            builder.Entity<ProductCompareList>()
                .HasKey(k => new { k.ProductId, k.CompareListId });

            builder.Entity<ProductCompareList>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductCompareLists)
                        .HasForeignKey(c => c.ProductId);

            builder.Entity<ProductCompareList>()
                .HasOne(c => c.CompareList)
                    .WithMany(pc => pc.ProductCompareLists)
                        .HasForeignKey(c => c.CompareListId);

            //ProductOrder
            builder.Entity<ProductOrder>()
                .HasKey(k => new { k.ProductId, k.OrderId });

            builder.Entity<ProductOrder>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductOrders)
                        .HasForeignKey(c => c.ProductId);

            builder.Entity<ProductOrder>()
                .HasOne(c => c.Order)
                    .WithMany(pc => pc.ProductOrders)
                        .HasForeignKey(c => c.OrderId);


            //Laguage Relationship
            //MenuLanguage
            builder.Entity<MenuLanguage>()
                .HasKey(k => new { k.LanguageId, k.MenuId });

            builder.Entity<MenuLanguage>()
                .HasOne(p => p.Menu)
                    .WithMany(pc => pc.MenuLanguages)
                        .HasForeignKey(c => c.MenuId);

            builder.Entity<MenuLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.MenuLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //SubMenuLanguage
            builder.Entity<SubMenuLanguage>()
                .HasKey(k => new { k.LanguageId, k.SubMenuId });

            builder.Entity<SubMenuLanguage>()
                .HasOne(p => p.SubMenu)
                    .WithMany(pc => pc.SubMenuLanguages)
                        .HasForeignKey(c => c.SubMenuId);

            builder.Entity<SubMenuLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.SubMenuLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //HeadMenuLanguage
            builder.Entity<HeadMenuLanguage>()
                .HasKey(k => new { k.LanguageId, k.HeadMenuId });

            builder.Entity<HeadMenuLanguage>()
                .HasOne(p => p.HeadMenu)
                    .WithMany(pc => pc.HeadMenuLanguages)
                        .HasForeignKey(c => c.HeadMenuId);

            builder.Entity<HeadMenuLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.HeadMenuLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //Do Username Unique
            builder.Entity<AppUser>(x => 
            {
                x.HasIndex(z => z.UserName).IsUnique();
            });


            //SlideLanguage
            builder.Entity<SlideLanguage>()
                .HasKey(k => new { k.LanguageId, k.SlideId });

            builder.Entity<SlideLanguage>()
                .HasOne(p => p.Slide)
                    .WithMany(pc => pc.SlideLanguages)
                        .HasForeignKey(c => c.SlideId);

            builder.Entity<SlideLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.SlideLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //AccountMenuLanguage
            builder.Entity<AccountMenuLanguage>()
                .HasKey(k => new { k.LanguageId, k.AccountMenuId });

            builder.Entity<AccountMenuLanguage>()
                .HasOne(p => p.AccountMenu)
                    .WithMany(pc => pc.AccountMenuLanguages)
                        .HasForeignKey(c => c.AccountMenuId);

            builder.Entity<AccountMenuLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.AccountMenuLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //ContactAboutLanguage
            builder.Entity<ContactAboutLanguage>()
                .HasKey(k => new { k.LanguageId, k.ContactAboutId });

            builder.Entity<ContactAboutLanguage>()
                .HasOne(p => p.ContactAbout)
                    .WithMany(pc => pc.ContactAboutLanguages)
                        .HasForeignKey(c => c.ContactAboutId);

            builder.Entity<ContactAboutLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.ContactAboutLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //LatestNewsLanguage
            builder.Entity<LatestNewsLanguage>()
                .HasKey(k => new { k.LanguageId, k.LatestNewsId });

            builder.Entity<LatestNewsLanguage>()
                .HasOne(p => p.LatestNews)
                    .WithMany(pc => pc.LatestNewsLanguages)
                        .HasForeignKey(c => c.LatestNewsId);

            builder.Entity<LatestNewsLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.LatestNewsLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //PageAboutLanguage
            builder.Entity<PageAboutLanguage>()
                .HasKey(k => new { k.LanguageId, k.PageAboutId });

            builder.Entity<PageAboutLanguage>()
                .HasOne(p => p.PageAbout)
                    .WithMany(pc => pc.PageAboutLanguages)
                        .HasForeignKey(c => c.PageAboutId);

            builder.Entity<PageAboutLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.PageAboutLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //PersonAboutLanguage
            builder.Entity<PersonAboutLanguage>()
                .HasKey(k => new { k.LanguageId, k.PersonAboutId });

            builder.Entity<PersonAboutLanguage>()
                .HasOne(p => p.PersonAbout)
                    .WithMany(pc => pc.PersonAboutLanguages)
                        .HasForeignKey(c => c.PersonAboutId);

            builder.Entity<PersonAboutLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.PersonAboutLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //ProductLanguage
            builder.Entity<ProductLanguage>()
                .HasKey(k => new { k.LanguageId, k.ProductId });

            builder.Entity<ProductLanguage>()
                .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductLanguages)
                        .HasForeignKey(c => c.ProductId);

            builder.Entity<ProductLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.ProductLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //ColorLanguage
            builder.Entity<ColorLanguage>()
                .HasKey(k => new { k.LanguageId, k.ColorId });

            builder.Entity<ColorLanguage>()
                .HasOne(p => p.Color)
                    .WithMany(pc => pc.ColorLanguages)
                        .HasForeignKey(c => c.ColorId);

            builder.Entity<ColorLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.ColorLanguages)
                        .HasForeignKey(c => c.LanguageId);

            //CategoryLanguage
            builder.Entity<CategoryLanguage>()
                .HasKey(k => new { k.LanguageId, k.CategoryId });

            builder.Entity<CategoryLanguage>()
                .HasOne(p => p.Category)
                    .WithMany(pc => pc.CategoryLanguages)
                        .HasForeignKey(c => c.CategoryId);

            builder.Entity<CategoryLanguage>()
                .HasOne(c => c.Language)
                    .WithMany(pc => pc.CategoryLanguages)
                        .HasForeignKey(c => c.LanguageId);
                
                    

            base.OnModelCreating(builder);
        }

    }
}

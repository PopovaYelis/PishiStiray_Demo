using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishiStiray_Demo.Entities
{
    public partial class Product
    {
        int itemCounter = 0;
        public string DiscountText
        {
            get
            {
                if (ProductDiscountAmount == 0)
                    return "";
                else
                    return "Cкидка " + ProductDiscountAmount.ToString("N2") + "%";
            }
        }
        public string HasDiscountVisibility
        {
            get
            {
                if (ProductDiscountAmount == 0)
                    return "Collapsed";
                else
                    return "Visible";
            }
        }
        public double CostWithDiscount
        {
            get
            {
                if (ProductDiscountAmount > 0)
                {
                    return (double)ProductCost * (double)(1 - ProductDiscountAmount / 100);
                }
                else
                    return (double)ProductCost;
            }
        }
        public string TotalCostText
        {
            get
            {
                return CostWithDiscount.ToString("N2") + $" рублей за {ProductUnit1.UnitName}";
            }
        }
        public string NameManufactor
        {
            get
            {
                return Manufacturer.ManufacturerName.ToString();
            }
        }
        public byte[] ImageType
        {
            get
            {
                if (ProductPhoto != null)
                    return ProductPhoto;
                else
                {
                    return File.ReadAllBytes("G:\\Demo\\picture.png");
                }
            }
        }
        public string BackColor
        {
            get
            {
                if (ProductDiscountAmount > 15)
                    return "#7fff00";
                else
                    return "White";
            }
        }

        public string ProductCount
        {
            get 
            {
                return ItemCounter.ToString();
            }
            set
            {
                ItemCounter = Convert.ToInt32(value);
            }
        }
        public int ItemCounter
        {
            get
            {
                return itemCounter;
            }
            set
            {
                itemCounter = value;
            }
        }
        //public static string AdminControlsVisibility
        //{
        //    //get
        //    //{
        //    //    if (App.CurrentUser.UserRole == 1)
        //    //    {
        //    //        return "Visible";
        //    //    }
        //    //    else
        //    //    {
        //    //        return "Hidden";
        //    //    }
        //    //}
        //}
    }
}

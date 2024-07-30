using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sait.domein.Entities;
using sait.domein.Abstract;

namespace sait.domein.Concrete
{
    public class EFClothesRepository : ClothesRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Clothe> Clothes
        {
            get { return context.Clothes; }
        }
        public void SaveClothes(Clothe clothe)
        {
            if (clothe.Id == 0)
                context.Clothes.Add(clothe);
            else
            {
               Clothe dbEntry = context.Clothes.Find(clothe.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = clothe.Name;
                    dbEntry.Description = clothe.Description;
                    dbEntry.Price = clothe.Price;
                    dbEntry.Category = clothe.Category;
                    dbEntry.ImageData = clothe.ImageData;
                    dbEntry.ImageMimeType = clothe.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Clothe DeleteClothe(int id)
        {
            Clothe dbEntry = context.Clothes.Find(id);
            if (dbEntry != null)
            {
                context.Clothes.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}

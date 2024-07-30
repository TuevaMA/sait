using sait.domein.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sait.domein.Abstract
{
    public interface ClothesRepository 
    {
        IEnumerable<Clothe> Clothes { get; }
        void SaveClothes(Clothe clothe);
        Clothe DeleteClothe(int id);
    }
}

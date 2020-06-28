using DoAnCuoiKiNhom5_SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class Tagcontroller
    {
        public static bool AddTag(Tag tag)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.Tags.Add(tag);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<Tag> LoadTag()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var tag = (from u in _context.Tags
                           select u).ToList();
                return tag;
            }
        }
        public static bool DeleteTag(Tag tag)
        {
            //try
            //{
                using (var _context = new SimpleNoteEntities())
                {
                    var tags = (from u in _context.Tags
                                where u.Tag1 == tag.Tag1
                                select u).SingleOrDefault();
                    _context.Tags.Remove(tags);
                    _context.SaveChanges();
                    return true;
                }
            //}
            //catch
            //{
            //    return false;
            //}
         }
    }
}

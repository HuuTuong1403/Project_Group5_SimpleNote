using DoAnCuoiKiNhom5_SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class Tag_Notecontroller
    {
        public static bool AddNote_Tag(Note_Tag note_tag)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.Note_Tag.Add(note_tag);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public static bool DeleteNote_Tag(Note_Tag note_tag)
        {
                try
                {
                    using (var _context = new SimpleNoteEntities())
                {
                    var note = (from u in _context.Note_Tag
                                where u.ID == note_tag.ID
                                where u.MiniTag == note_tag.MiniTag
                                select u).SingleOrDefault();
                    _context.Note_Tag.Remove(note);
                    _context.SaveChanges();
                    return true;
                }
                }
                catch
                {
                    return false;
                }
        }

        public static List<Note_Tag> getListNote_searchTag(string tag)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note_tag = (from u in _context.Note_Tag.AsEnumerable()
                            where u.MiniTag.Contains(tag)
                            select new Note_Tag
                            {
                                ID = u.ID,
                                MiniTag = u.MiniTag
                            }).ToList();
                return note_tag;

            }
        }

        public static List<Note_Tag> LoadNoteofTag(string tag)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Note_Tag
                            where u.MiniTag == tag
                            select u).ToList();
                return note;
            }

        }

        public static List<Note_Tag> GetTag(int ID)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note_tag = (from u in _context.Note_Tag
                            where u.ID == ID
                            select u).ToList();
                return note_tag;
            }
        }

    }
}

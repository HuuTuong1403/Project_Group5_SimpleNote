using DoAnCuoiKiNhom5_SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class Note_backupcontroller
    {
        public static bool AddNote_Backup(Note_Backup note_backup)
        {
                try
                {
                    using (var _context = new SimpleNoteEntities())
                    {
                        _context.Note_Backup.Add(note_backup);
                        _context.SaveChanges();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }

        }

        public static Note_Backup LoadNote_Backup(int ID)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note_backup = (from u in _context.Note_Backup
                            where u.ID == ID
                            select u).SingleOrDefault();
                return note_backup;
            }
        }

        public static bool DeleteNote_Backup(Note_Backup note_backup)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    var notes = (from u in _context.Note_Backup
                                 where u.ID == note_backup.ID
                                 select u).SingleOrDefault();
                    _context.Note_Backup.Remove(notes);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

using DoAnCuoiKiNhom5_SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class User_Note_backupcontroller
    {
        public static bool AddUser_Note_Backup(User_Note_Backup user_note_backup)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.User_Note_Backup.Add(user_note_backup);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public static List<User_Note_Backup> LoadNoteofUser_Backup(string username)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var user_note_backup = (from u in _context.User_Note_Backup
                            where u.Username == username
                            select u).ToList();
                return user_note_backup;
            }

        }

        public static bool DeleteUser_Note_Backup(User_Note_Backup user_note_backup)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    var user_notes = (from u in _context.User_Note_Backup
                                      where u.ID == user_note_backup.ID
                                      where u.Username == user_note_backup.Username
                                      select u).SingleOrDefault();
                    _context.User_Note_Backup.Remove(user_notes);
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

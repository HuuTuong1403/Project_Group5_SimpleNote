using DoAnCuoiKiNhom5_SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class User_Notecontrollers
    {
        public static bool AddUser_Note(User_Note user_note)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.User_Note.Add(user_note);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        public static bool UpdateUser_Note(User_Note user_note)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.User_Note.AddOrUpdate(user_note);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        public static List<User_Note> LoadNoteofUser(string username)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.User_Note
                            where u.Username == username
                            select u).ToList();
                return note;
            }
           
        }

        public static bool DeleteUser_Note(User_Note user_note)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    var user_notes = (from u in _context.User_Note
                                      where u.ID == user_note.ID
                                      where u.Username == user_note.Username
                                      select u).SingleOrDefault();
                    _context.User_Note.Remove(user_notes);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        public static bool checkUser_Note(User_Note user_note)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var user_notes = (from u in _context.User_Note
                                  where u.ID == user_note.ID
                                  where u.Username == user_note.Username
                                  select u).SingleOrDefault();
                if (user_notes != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
        }

    }
}

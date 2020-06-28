using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAnCuoiKiNhom5_SimpleNote.Models;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class UserControllers
    {
        public static bool AddUser(User user)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        public static User getUser(string username)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var user = (from u in _context.Users
                            where u.Username == username
                            select u).ToList();
                if (user.Count == 1)
                {
                    return user[0];
                }
                else
                {
                    return null;
                }
            }
        }
        public static bool checkPassword(string username,string password)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var user = (from u in _context.Users
                            where u.Username == username && u.Password==password
                            select u).SingleOrDefault();
                if (user is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



    }
}

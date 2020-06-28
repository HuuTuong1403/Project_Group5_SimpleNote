using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKiNhom5_SimpleNote.Models;

namespace DoAnCuoiKiNhom5_SimpleNote.Controllers
{
    public class Notecontrollers
    {
        public static int getIDNotes()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Notes.AsEnumerable()
                            select new Note
                            {
                                ID = u.ID,
                                Header = u.Header,
                                Context = u.Context,
                                Time = u.Time,
                            }).ToList();
                if (note.Count <= 0)
                {
                    return 0;
                }
                return note[note.Count - 1].ID;

            }
        }
        public static List<int> getListNoteID()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Notes.AsEnumerable()
                            select u.ID).ToList();
                if (note.Count <= 0)
                {
                    return null;
                }
                return note;

            }
        }
        public static bool AddNote(Note note)
        {
                try
                {
                    using (var _context = new SimpleNoteEntities())
                    {
                        _context.Notes.Add(note);
                        _context.SaveChanges();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
        }
        public static bool UpdateNote(Note note)
        {
            if (note!=null)
            {
                    try
                    {
                        using (var _context = new SimpleNoteEntities())
                        {
                            _context.Notes.AddOrUpdate(note);
                            _context.SaveChanges();
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
            }
            return false;
        }
        public static Note LoadNote(int ID)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Notes
                            where u.ID == ID
                            select u).SingleOrDefault();
                return note;
            }
        }
        //public static Note GetNote(string Header)
        //{
        //    using (var _context = new SimpleNoteEntities())
        //    {
        //        var note = (from u in _context.Notes
        //                    where u.Header == Header
        //                    select u).FirstOrDefault();
        //        return note;
        //    }
        //}
        public static Note GetNote(int ID)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Notes
                            where u.ID == ID
                            select u).FirstOrDefault();
                return note;
            }
        }
        public static bool DeleteNote(Note note)
        {
            try
            {
                using (var _context = new SimpleNoteEntities())
                {
                    var notes = (from u in _context.Notes
                                 where u.ID == note.ID
                                 select u).SingleOrDefault();
                    _context.Notes.Remove(notes);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<Note> getListNote_searchHeader(string header)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var note = (from u in _context.Notes.AsEnumerable()
                            where u.Header.Contains(header)
                            select new Note
                            {
                                ID = u.ID,
                                Context = u.Context,
                                Header = u.Header,
                                Time = u.Time,
                                TimeEdit = u.TimeEdit,
                            }).ToList();
                return note;

            }
        }
    }
}

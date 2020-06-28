using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKiNhom5_SimpleNote.Controllers;
using DoAnCuoiKiNhom5_SimpleNote.Models;
using Guna.UI.WinForms;
namespace DoAnCuoiKiNhom5_SimpleNote.Views
{
    public partial class frmNote : Form
    {
        #region Variable
        private Note note;
        private User_Note user_note;
        private Stack<string> undoList = new Stack<string>();
        private List<User_Note> ListUser_Notes;
        private List<Note> ListNotes;
        private List<Note> listNoteSearchHeader;
        private List<Note_Tag> listNoteSearchTag;
        private List<string> listTag;
        private List<Note_Backup> ListNotes_Backup;
        private List<User_Note_Backup> ListUser_Notes_Backup;
        private List<Note_Tag> note_tag = new List<Note_Tag>();
        private List<Note> notetemp = new List<Note>();
        private List<Tag> TagSort;

        private int ID;
        private int cButton = 3;
        private int t = 15;

        private string[] tag = new string[100];
        private string text_header_save;
        private string text_save;
        private string format = "MMM d, yyyy HH:mm tt";
        private string Username;
        private string text_print;

        //Biến kiểm tra
        private bool flaq = true;
        private bool flaqMenu = true;
        private bool flaq1 = true;
        private bool flaq2 = true;
        private bool checkSearch = false;
        private bool flaqInfo = true;
        private bool checkDateCreated = true;
        private bool checkAlphabeticalHeader = true;
        private bool checkReversed = true;
        private bool checkEdit = true;
        private bool checkTagSort = true;
        #endregion

        public frmNote(string username)
        {
            InitializeComponent();
            ListUser_Notes = new List<User_Note>();
            ListNotes = new List<Note>();
            ID = Notecontrollers.getIDNotes() + 1;
            Username = username;
            note = new Note();
            user_note = new User_Note();

            ListNotes_Backup = new List<Note_Backup>();
            ListUser_Notes_Backup = new List<User_Note_Backup>();

            listNoteSearchHeader = new List<Note>();
            listNoteSearchTag = new List<Note_Tag>();
            listTag = new List<string>();
            TagSort = new List<Tag>();
        }

        #region Method
        private void ShowMenu()
        {
            this.pnMenu.Visible = true;
            this.pnAll.Location = new Point(0 + pnMenu.Width, 24);
            this.btnColor.Visible = false;
            this.btnFont.Visible = false;
            this.ptBOpacity.Visible = true;

        }
        private void HideMenu()
        {
            this.btnColor.Visible = false;
            this.btnFont.Visible = false;
            this.pnMenu.Visible = false;
            this.pnAll.Location = new Point(0, 27);
            this.ptBOpacity.Visible = false;
        }
        private void ShowInfo()
        {
            this.pnAll.Location = new Point(0 - pnInfo.Width, 24);
            this.pnInfo.Location = new Point(821, 24);
            this.pnInfo.Visible = true;
            this.btnColor.Visible = false;
            this.btnFont.Visible = false;
            this.ptBOpacity.Visible = true;
        }
        private void HideInfo()
        {
            this.pnAll.Location = new Point(0, 24);
            this.ptBOpacity.Visible = false;
            this.btnColor.Visible = true;
            this.btnFont.Visible = true;
            this.pnInfo.Visible = false;
        }
        private void TrashShow()
        {
            this.rtb_Note.Visible = false;
            this.txt_Header.Visible = false;
            this.pnAddTag.Visible = false;
            this.btnNewNote.Enabled = false;
        }
        private void TrashHide()
        {
            this.pnAddTag.Visible = false;
            this.rtb_Note.Visible = false;
            this.txt_Header.Visible = false;
            this.btnNewNote.Enabled = true;
        }
        private int CountWord(string text)
        {
            string str;
            int count = 1, l = 0;
            str = text.Trim();
            int c = 0;
            if (text.Length == 0)
            {
                return c;
            }
            while (l < str.Length - 1)
            {
                if (str[l] == ' ' && str[l - 1] != ' ')
                {
                    count++;
                }
                l++;
            }
            return count;
        }
        private int CountCharacter(string text)
        {
            string str;
            int count = 1, l = 0;
            int c = 0;
            if (text.Length == 0)
            {
                return c;
            }
            str = text.Trim();
            while (l < str.Length - 1)
            {
                count++;
                l++;
            }
            return count;
        }
        private void DesignButton(GunaButton btn)
        {
            btn.Location = new System.Drawing.Point(cButton, 3);
            cButton += 101;
            btn.Size = new System.Drawing.Size(95, 21);
            btn.Radius = 10;
            btn.BaseColor = Color.FromArgb(64, 64, 64);
            btn.Image = null;
            btn.OnHoverBaseColor = Color.FromArgb(128, 255, 128);
            btn.Font = new Font("Times New Roman", 11);
            btn.DoubleClick += Btn_DoubleClick;
        }
        #endregion

        #region Event
        private void tsmNewNote_Click(object sender, EventArgs e)
        {
            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
            {
                ID += 1;
                note.Header = "New note...";
                note.Context = "";
                note.ID = ID;
                string format = "MMM d, yyyy HH:mm tt";
                note.Time = DateTime.Now.ToString(format);
                note.TimeEdit = DateTime.Now.ToString(format);
                user_note.Username = Username;
                user_note.ID = note.ID;
                this.rtb_Note.Visible = true;
                this.txt_Header.Visible = true;
                this.lst_Note.Items.Add(note.ToString());
                this.ListUser_Notes.Add(user_note);
                this.ListNotes.Add(note);
                if (this.lst_Note.SelectedItems.Count > 0 && this.lst_Note.SelectedItems[0].Text == "New note...")
                {
                    this.pnAddTag.Controls.Clear();
                    this.txtAddTag.Clear();
                    this.pnAddTag.Controls.Add(this.txtAddTag);
                    this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                    this.txt_Header.Clear();
                    this.rtb_Note.Clear();
                    this.txt_Header.TextChanged += Txt_Header_TextChanged;
                    this.rtb_Note.TextChanged += rtb_Note_TextChanged;
                    this.txtAddTag.Visible = true;
                    this.pnAddTag.Visible = true;

                }
                else
                {
                    MessageBox.Show("Mời chọn note để ghi chú","Thông báo",MessageBoxButtons.OK);
                }
                HideMenu();
                flaqMenu = true;
            }
        }

        private void Txt_Header_TextChanged(object sender, EventArgs e)
        {
            if (this.txt_Header.TextLength > 0)
            {
                if (this.lst_Note.SelectedItems.Count > 0)
                {
                    if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
                    {
                        foreach (ListViewItem item in this.lst_Note.SelectedItems)
                        {
                            ListNotes[item.Index].Header = this.txt_Header.Text.Trim();
                            ListNotes[item.Index].TimeEdit = DateTime.Now.ToString(format);
                            user_note.ID = ListNotes[item.Index].ID;
                            user_note.Username = Username;
                            User_Notecontrollers.UpdateUser_Note(user_note);
                            Notecontrollers.UpdateNote(ListNotes[item.Index]);
                            item.Text = ListNotes[item.Index].Header;
                        }
                    }
                    if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                    {
                        foreach (ListViewItem item in this.lst_Note.SelectedItems)
                        {
                            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                            {
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Header = this.txt_Header.Text.Trim();
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].TimeEdit = DateTime.Now.ToString(format);
                                user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                                user_note.Username = Username;
                                User_Notecontrollers.UpdateUser_Note(user_note);
                                Notecontrollers.UpdateNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2]);
                                item.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Header;
                            }
                            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                            {
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Header = this.txt_Header.Text.Trim();
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].TimeEdit = DateTime.Now.ToString(format);
                                user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                                user_note.Username = Username;
                                User_Notecontrollers.UpdateUser_Note(user_note);
                                Notecontrollers.UpdateNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1]);
                                item.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Header;
                            }
                            if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                            {
                                notetemp[item.Index].Header = this.txt_Header.Text.Trim();
                                notetemp[item.Index].TimeEdit = DateTime.Now.ToString(format);
                                user_note.ID = notetemp[item.Index].ID;
                                user_note.Username = Username;
                                User_Notecontrollers.UpdateUser_Note(user_note);
                                Notecontrollers.UpdateNote(notetemp[item.Index]);
                                item.Text = notetemp[item.Index].Header;
                            }
                        }
                       
                    }
                }
                else
                {
                    MessageBox.Show("Mời chọn note để ghi chú", "Thông báo", MessageBoxButtons.OK);
                }
            }
            
        }

        private void frmNote_Load(object sender, EventArgs e)
        {
            ListUser_Notes = User_Notecontrollers.LoadNoteofUser(Username);
            for (int i = 0; i < this.ListUser_Notes.Count(); i++)
            {
                this.ListNotes.Add(Notecontrollers.LoadNote(this.ListUser_Notes[i].ID));
            }
            for (int j = 0; j < this.ListNotes.Count(); j++)
            {
                this.lst_Note.Items.Add(this.ListNotes[j].Header);
            }
            this.pnAddTag.Visible = false;
            this.btnAllNotes.ForeColor = Color.Black;
            this.btnAllNotes.OnHoverForeColor = Color.Black;
            flaq1 = false;
            toolTip1.SetToolTip(this.btnNewNote, "New Note");
            toolTip1.SetToolTip(this.btnMenu, "Menu");
            toolTip1.SetToolTip(this.swtToggle, "Toggle Sidebar");
            toolTip1.SetToolTip(this.btnDelete, "Trash");
            toolTip1.SetToolTip(this.btnInfo, "Info");
            toolTip1.SetToolTip(this.btnDeleteTag, "Delete Tag");
            toolTip1.SetToolTip(this.btnColor, "Change Color");
            toolTip1.SetToolTip(this.btnFont, "Change Font");
            toolTip1.SetToolTip(this.btnDeleteForever, "Delete Forever");
            toolTip1.SetToolTip(this.btnRestoreNote, "Restore Note");
            toolTip1.SetToolTip(this.btnAllNotes, "View All Note");
            toolTip1.SetToolTip(this.btnTrash, "View Trash");
            toolTip1.SetToolTip(this.btnExit, "Close Info");
            this.Size = new Size(1014, 648);
        }
        
        private void Btn_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lst_Note.SelectedItems)
            {
                GunaButton btn = sender as GunaButton;
                string temp = "";
                int x = btn.Location.X;
                List<GunaButton> listbutton = new List<GunaButton>();
                int count = 0;
                int width = btn.Width;
                foreach (GunaButton itembtn in this.pnAddTag.Controls.OfType<GunaButton>())
                {
                    if (itembtn != btn)
                    {
                        listbutton.Add(itembtn);
                        temp = temp + itembtn.Text.Trim() + " ";
                    }
                }
                Note_Tag note_tag = new Note_Tag();
                if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false )
                {
                    note_tag.ID = this.ListNotes[item.Index].ID;
                    note_tag.MiniTag = btn.Text;
                    if (Tag_Notecontroller.DeleteNote_Tag(note_tag) == false)
                    {
                        MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                }
                if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                {
                    if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                    {
                        note_tag.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                        note_tag.MiniTag = btn.Text;
                        if (Tag_Notecontroller.DeleteNote_Tag(note_tag) == false)
                        {
                            MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                        }
                    }
                    if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                    {
                        note_tag.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                        note_tag.MiniTag = btn.Text;
                        if (Tag_Notecontroller.DeleteNote_Tag(note_tag) == false)
                        {
                            MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                        }
                    }
                    if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                    {
                        note_tag.ID = notetemp[item.Index].ID;
                        note_tag.MiniTag = btn.Text;
                        if (Tag_Notecontroller.DeleteNote_Tag(note_tag) == false)
                        {
                            MessageBox.Show("Xóa không thành công", "Thông báo", MessageBoxButtons.OK);
                        }
                    }
                }
                this.pnAddTag.Controls.Remove(btn);
                cButton = cButton - 101;
                foreach (GunaButton itembtn in listbutton.OfType<GunaButton>())
                {
                    if (itembtn.Location.X > x)
                    {
                        count++;
                        itembtn.Location = new System.Drawing.Point(itembtn.Location.X - 101, itembtn.Location.Y);
                    }
                }

                if (listbutton.Count == 0)
                {
                    this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                }
                else
                {
                    this.txtAddTag.Location = new System.Drawing.Point(txtAddTag.Location.X - 101, -3);
                }
            }
        }

        private void txtAddTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (lst_Note.SelectedItems.Count >= 0)
            {
                foreach (ListViewItem item in this.lst_Note.SelectedItems)
                {
                    GunaButton btn = new GunaButton();
                    string temp = " ";
                    if (e.KeyCode == Keys.Enter)
                    {
                        if (this.txtAddTag.Text.Trim().Length <= 0)
                        {
                            return;
                        }
                        if (this.txtAddTag.Text.Trim().Contains(temp) == true)
                        {
                            this.txtAddTag.Clear();
                            MessageBox.Show("Hashtag không được có khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                            return;
                        }
                        DesignButton(btn);
                        btn.Text = this.txtAddTag.Text.Trim();

                        this.txtAddTag.Text = "";
                        Note_Tag note_tag = new Note_Tag();
                        if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
                        {
                            note_tag.ID = this.ListNotes[item.Index].ID;
                            this.ListNotes[item.Index].TimeEdit = DateTime.Now.ToString(format);
                        }
                        if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                        {
                            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                            {
                                note_tag.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].TimeEdit = DateTime.Now.ToString(format);
                            }
                            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                            {
                                note_tag.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                                this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].TimeEdit = DateTime.Now.ToString(format);
                            }
                            if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                            {
                                note_tag.ID = notetemp[item.Index].ID;
                                notetemp[item.Index].TimeEdit = DateTime.Now.ToString(format);
                            }
                        }
                        note_tag.MiniTag = btn.Text;
                        Tag tag = new Tag();
                        tag.Tag1 = btn.Text;
                        Tagcontroller.AddTag(tag);
                        Tag_Notecontroller.AddNote_Tag(note_tag);
                        this.txtAddTag.Location = new System.Drawing.Point(101 + btn.Location.X, -3);
                        this.pnAddTag.Controls.Add(btn);
                    }

                }
            }

        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtb_Note_TextChanged(object sender, EventArgs e)
        {
            if (this.lst_Note.SelectedItems.Count > 0)
            {
                this.lblCountWords.Text = CountWord(this.rtb_Note.Text).ToString() + " words";
                this.lblCountCharacter.Text = CountCharacter(this.rtb_Note.Text).ToString() + " characters";
                if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false )
                {
                    foreach (ListViewItem item in this.lst_Note.SelectedItems)
                    {
                        ListNotes[item.Index].Context = this.rtb_Note.Text.Trim();
                        ListNotes[item.Index].TimeEdit = DateTime.Now.ToString(format);
                        user_note.ID = ListNotes[item.Index].ID;
                        user_note.Username = Username;
                        User_Notecontrollers.UpdateUser_Note(user_note);
                        Notecontrollers.UpdateNote(ListNotes[item.Index]);

                    }
                }
                if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                {
                    foreach (ListViewItem item in this.lst_Note.SelectedItems)
                    {
                        if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                        {
                            this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Context = this.rtb_Note.Text.Trim();
                            this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].TimeEdit = DateTime.Now.ToString(format);
                            user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                            user_note.Username = Username;
                            User_Notecontrollers.UpdateUser_Note(user_note);
                            Notecontrollers.UpdateNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2]);
                        }
                        if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                        {
                            this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Context = this.rtb_Note.Text.Trim();
                            this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].TimeEdit = DateTime.Now.ToString(format);
                            user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                            user_note.Username = Username;
                            User_Notecontrollers.UpdateUser_Note(user_note);
                            Notecontrollers.UpdateNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1]);
                        }
                        if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                        {
                            notetemp[item.Index].Context = this.rtb_Note.Text.Trim();
                            notetemp[item.Index].TimeEdit = DateTime.Now.ToString(format);
                            user_note.ID = notetemp[item.Index].ID;
                            user_note.Username = Username;
                            User_Notecontrollers.UpdateUser_Note(user_note);
                            Notecontrollers.UpdateNote(notetemp[item.Index]);
                        }
                    }
                }
                if (rtb_Note.Text != null)
                {
                    undoList.Push(this.rtb_Note.Text);
                    rtb_Note.SelectionStart = this.rtb_Note.Text.Length;
                }
            }
            else
            {
                MessageBox.Show("Mời chọn note để ghi chú", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void lst_Note_Click(object sender, EventArgs e)
        {
            this.pnAddTag.Controls.Clear();
            cButton = 3;
            this.txtAddTag.Clear();
            this.pnAddTag.Visible = true;
            this.pnAddTag.Controls.Add(this.txtAddTag);
            this.txtAddTag.Location = new System.Drawing.Point(0, -3);
            this.txtAddTag.Visible = true;
            this.btnDelete.Visible = true;
            this.btnInfo.Visible = true;
            this.btnColor.Visible = true;
            this.btnFont.Visible = true;
            this.txt_Header.Enabled = true;
            this.rtb_Note.Enabled = true;
            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && this.txtSearchNotes.TextLength <= 0)
            {
                foreach (ListViewItem item in this.lst_Note.SelectedItems)
                {

                    this.txt_Header.Visible = true;
                    this.txt_Header.Text = this.ListNotes[item.Index].Header;
                    this.rtb_Note.Visible = true;
                    this.rtb_Note.Text = this.ListNotes[item.Index].Context;
                    this.btnDelete.Visible = true;
                    this.btnInfo.Visible = true;
                    this.lblCountWords.Text = CountWord(this.ListNotes[item.Index].Context).ToString() + " words";
                    this.lblCountCharacter.Text = CountCharacter(this.ListNotes[item.Index].Context).ToString() + " characters";
                    this.pnAddTag.Visible = true;
                    this.lblShowDate.Text = this.ListNotes[item.Index].TimeEdit;
                    this.lblShowDateCreated.Text = this.ListNotes[item.Index].Time;
                    List<Note_Tag> note_tag_click = Tag_Notecontroller.GetTag(this.ListNotes[item.Index].ID);

                    for (int i = 0; i < note_tag_click.Count(); i++)
                    {
                        GunaButton btn = new GunaButton();
                        DesignButton(btn);
                        btn.Text = note_tag_click[i].MiniTag;
                        this.txtAddTag.Location = new System.Drawing.Point(101 + btn.Location.X, -3);
                        this.pnAddTag.Controls.Add(btn);
                    }
                    //Print and Save note
                    text_print = "Title: " + "\t" + this.ListNotes[item.Index].Header + "\n\n" + "Context: " + "\n" + this.ListNotes[item.Index].Context;
                    text_save = "Title: " + "\t" + this.ListNotes[item.Index].Header + "\n\n" + "Context: " + "\n" + this.ListNotes[item.Index].Context + "\n\n" + "Tags: " + "\n\t";
                    for (int k = 0; k < note_tag_click.Count(); k++)
                    {
                        text_save += note_tag_click[k].MiniTag + " ";
                    }
                    text_header_save = this.ListNotes[item.Index].Header;


                    if (narrowToolStripMenuItem.Checked)
                    {
                        if (lst_Note.SelectedItems.Count > 0)
                        {
                            this.rtb_Note.SelectionIndent = 50;
                        }
                    }
                    if (fullToolStripMenuItem.Checked)
                    {
                        if (lst_Note.SelectedItems.Count > 0)
                        {
                            this.rtb_Note.SelectionIndent = 0;
                        }
                    }
                }
            }
            if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
            {

                foreach (ListViewItem item in this.lst_Note.SelectedItems)
                {
                    if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                    {
                        this.txt_Header.Visible = true;
                        this.txt_Header.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Header; //Lỗi ở đây nè 
                        this.rtb_Note.Visible = true;
                        this.rtb_Note.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Context;

                        this.lblCountWords.Text = CountWord(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Context).ToString() + " words";
                        this.lblCountCharacter.Text = CountCharacter(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Context).ToString() + " characters";
                        this.pnAddTag.Visible = true;
                        this.lblShowDate.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].TimeEdit;

                        List<Note_Tag> note_tag_click = Tag_Notecontroller.GetTag(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID);
                        for (int i = 0; i < note_tag_click.Count(); i++)
                        {
                            GunaButton btn = new GunaButton();
                            DesignButton(btn);
                            btn.Text = note_tag_click[i].MiniTag;
                            this.txtAddTag.Location = new System.Drawing.Point(101 + btn.Location.X, -3);
                            this.pnAddTag.Controls.Add(btn);
                        }
                        if (narrowToolStripMenuItem.Checked)
                        {
                            if (lst_Note.SelectedItems.Count > 0)
                            {
                                this.rtb_Note.SelectionIndent = 50;
                            }
                        }
                        if (fullToolStripMenuItem.Checked)
                        {
                            if (lst_Note.SelectedItems.Count > 0)
                            {
                                this.rtb_Note.SelectionIndent = 0;
                            }
                        }

                    }//An vo search
                    else
                    {
                        if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                        {
                            this.txt_Header.Visible = true;
                            this.txt_Header.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Header; //Lỗi ở đây nè 
                            this.rtb_Note.Visible = true;
                            this.rtb_Note.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Context;

                            this.lblCountWords.Text = CountWord(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Context).ToString() + " words";
                            this.lblCountCharacter.Text = CountCharacter(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Context).ToString() + " characters";
                            this.pnAddTag.Visible = true;
                            this.lblShowDate.Text = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].TimeEdit;

                            List<Note_Tag> note_tag_click = Tag_Notecontroller.GetTag(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID);
                            for (int i = 0; i < note_tag_click.Count(); i++)
                            {
                                GunaButton btn = new GunaButton();
                                DesignButton(btn);
                                btn.Text = note_tag_click[i].MiniTag;
                                this.txtAddTag.Location = new System.Drawing.Point(101 + btn.Location.X, -3);
                                this.pnAddTag.Controls.Add(btn);
                            }
                            if (narrowToolStripMenuItem.Checked)
                            {
                                if (lst_Note.SelectedItems.Count > 0)
                                {
                                    this.rtb_Note.SelectionIndent = 50;
                                }
                            }
                            if (fullToolStripMenuItem.Checked)
                            {
                                if (lst_Note.SelectedItems.Count > 0)
                                {
                                    this.rtb_Note.SelectionIndent = 0;
                                }
                            }

                        }
                        else
                        {
                            if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                            {
                                this.txt_Header.Visible = true;
                                this.txt_Header.Text = notetemp[item.Index].Header;
                                this.rtb_Note.Visible = true;
                                this.rtb_Note.Text = notetemp[item.Index].Context;

                                this.lblCountWords.Text = CountWord(notetemp[item.Index].Context).ToString() + " words";
                                this.lblCountCharacter.Text = CountCharacter(notetemp[item.Index].Context).ToString() + " characters";
                                this.pnAddTag.Visible = true;
                                this.lblShowDate.Text = notetemp[item.Index].TimeEdit;

                                List<Note_Tag> note_tag_click = Tag_Notecontroller.GetTag(this.notetemp[item.Index].ID);
                                for (int i = 0; i < note_tag_click.Count(); i++)
                                {
                                    GunaButton btn = new GunaButton();
                                    DesignButton(btn);
                                    btn.Text = note_tag_click[i].MiniTag;
                                    this.txtAddTag.Location = new System.Drawing.Point(101 + btn.Location.X, -3);
                                    this.pnAddTag.Controls.Add(btn);
                                }
                                if (narrowToolStripMenuItem.Checked)
                                {
                                    if (lst_Note.SelectedItems.Count > 0)
                                    {
                                        this.rtb_Note.SelectionIndent = 50;
                                    }
                                }
                                if (fullToolStripMenuItem.Checked)
                                {
                                    if (lst_Note.SelectedItems.Count > 0)
                                    {
                                        this.rtb_Note.SelectionIndent = 0;
                                    }
                                }

                            }
                            else
                            {
                                this.txtSearchNotes.Text = item.Text;
                            }
                        }
                    }      
                }
            }
            if (this.txtSearchNotes.WaterMark.Contains("Trash"))
            {
                this.btnDelete.Visible = false;
                this.btnInfo.Visible = false;
                this.pnAddTag.Visible = false;
                foreach (ListViewItem item in this.lst_Note.SelectedItems)
                {
                    this.txt_Header.Enabled = false;
                    this.rtb_Note.Enabled = false;
                    this.btnDeleteForever.Visible = true;
                    this.btnRestoreNote.Visible = true;
                    this.btnColor.Visible = false;
                    this.btnFont.Visible = false;

                    this.txtAddTag.Visible = false;
                    this.txt_Header.Visible = true;
                    this.txt_Header.Text = this.ListNotes_Backup[item.Index].Header;
                    this.rtb_Note.Visible = true;
                    this.rtb_Note.Text = this.ListNotes_Backup[item.Index].Context;

                    this.lblCountWords.Text = CountWord(this.ListNotes_Backup[item.Index].Context).ToString() + " words";
                    this.lblCountCharacter.Text = CountCharacter(this.ListNotes_Backup[item.Index].Context).ToString() + " characters";
                    this.pnAddTag.Visible = true;
                    this.lblShowDate.Text = this.ListNotes_Backup[item.Index].TimeEdit;
                    this.lblShowDateCreated.Text = this.ListNotes_Backup[item.Index].Time;

                    if (narrowToolStripMenuItem.Checked)
                    {
                        if (lst_Note.SelectedItems.Count > 0)
                        {
                            this.rtb_Note.SelectionIndent = 50;
                        }
                    }
                    if (fullToolStripMenuItem.Checked)
                    {
                        if (lst_Note.SelectedItems.Count > 0)
                        {
                            this.rtb_Note.SelectionIndent = 0;
                        }
                    }
                }
            }
        }

        private void tsmCut_Click(object sender, EventArgs e)
        {
            if (rtb_Note.SelectedText != String.Empty)
            {
                Clipboard.SetData(DataFormats.Text, rtb_Note.SelectedText);
            }
            rtb_Note.SelectedText = String.Empty;
        }

        private void tsmCopy_Click(object sender, EventArgs e)
        {
            rtb_Note.Copy();
        }

        private void tsmPaste_Click(object sender, EventArgs e)
        {
            int position = rtb_Note.SelectionStart;
            this.rtb_Note.Text = this.rtb_Note.Text.Insert(position, Clipboard.GetText());
        }

        private void tsmSelectAll_Click(object sender, EventArgs e)
        {
            rtb_Note.SelectAll();
        }

        private void tsmUndo_Click(object sender, EventArgs e)
        {
            try
            {
                undoList.Pop();
                this.rtb_Note.Text = undoList.Pop();
            }
            catch
            {
            }
            if (undoList.Count == 0)
            {
                this.rtb_Note.Text = "";

            }
        }

        private void btnNewNote_Click(object sender, EventArgs e)
        {
            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
            {
                ID += 1;
                note.Header = "New note...";
                note.Context = "";
                note.ID = ID;
                string format = "MMM d, yyyy HH:mm tt";
                note.Time = DateTime.Now.ToString(format);
                note.TimeEdit = DateTime.Now.ToString(format);
                user_note.Username = Username;
                user_note.ID = note.ID;
                this.rtb_Note.Visible = true;
                this.txt_Header.Visible = true;
                this.lst_Note.Items.Add(note.ToString());
                this.ListUser_Notes.Add(user_note);
                this.ListNotes.Add(note);
                if (this.lst_Note.SelectedItems.Count > 0 && this.lst_Note.SelectedItems[0].Text == "New note...")
                {
                    this.pnAddTag.Controls.Clear();
                    this.txtAddTag.Clear();
                    this.pnAddTag.Controls.Add(this.txtAddTag);
                    this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                    this.txt_Header.Clear();
                    this.rtb_Note.Clear();
                    this.txt_Header.TextChanged += Txt_Header_TextChanged;
                    this.rtb_Note.TextChanged += rtb_Note_TextChanged;
                    this.txtAddTag.Visible = true;
                    this.pnAddTag.Visible = true;

                }
                else
                {
                    MessageBox.Show("Mời chọn note để ghi chú", "Thông báo", MessageBoxButtons.OK);
                }
                HideMenu();
                flaqMenu = true;

            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (this.lst_Note.SelectedItems.Count > 0)
            {
                Note_Backup note_backup = new Note_Backup();
                User_Note_Backup user_note_backup = new User_Note_Backup();
                foreach (ListViewItem item in this.lst_Note.SelectedItems)
                {
                    if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
                    {
                        user_note_backup.Username = Username;
                        user_note_backup.ID = ListNotes[item.Index].ID;
                        note_backup.ID = ListNotes[item.Index].ID;
                        note_backup.Header = ListNotes[item.Index].Header;
                        note_backup.Context = ListNotes[item.Index].Context;
                        note_backup.Time = ListNotes[item.Index].Time;
                        note_backup.TimeEdit = ListNotes[item.Index].TimeEdit;

                        Note_backupcontroller.AddNote_Backup(note_backup);
                        User_Note_backupcontroller.AddUser_Note_Backup(user_note_backup);

                        user_note.ID = ListNotes[item.Index].ID;
                        user_note.Username = Username;

                        User_Notecontrollers.DeleteUser_Note(user_note);

                        List<Note_Tag> note_tag_delete = Tag_Notecontroller.GetTag(ListNotes[item.Index].ID);
                        for (int i = 0; i < note_tag_delete.Count(); i++)
                        {
                            Tag_Notecontroller.DeleteNote_Tag(note_tag_delete[i]);
                        }

                        Notecontrollers.DeleteNote(this.ListNotes[item.Index]);

                        this.ListNotes.RemoveAt(item.Index);
                        this.lst_Note.Items.Remove(item);
                    }
                    if (checkSearch == true || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                    {
                        if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() != 0)
                        {
                            user_note_backup.Username = Username;
                            user_note_backup.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                            note_backup.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                            note_backup.Header = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Header;
                            note_backup.Context = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Context;
                            note_backup.Time = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].Time;
                            note_backup.TimeEdit = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].TimeEdit;

                            Note_backupcontroller.AddNote_Backup(note_backup);
                            User_Note_backupcontroller.AddUser_Note_Backup(user_note_backup);
                            user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID;
                            user_note.Username = Username;

                            User_Notecontrollers.DeleteUser_Note(user_note);

                            List<Note_Tag> note_tag_delete = Tag_Notecontroller.GetTag(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2].ID);
                            for (int i = 0; i < note_tag_delete.Count(); i++)
                            {
                                Tag_Notecontroller.DeleteNote_Tag(note_tag_delete[i]);
                            }

                            Notecontrollers.DeleteNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 2]);
                            this.listNoteSearchHeader.RemoveAt(item.Index - this.listTag.Count() - 2);
                            this.lst_Note.Items.Remove(item);
                        }
                        else
                        {
                            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && item.Index > this.listTag.Count() && !(this.txtSearchNotes.Text.Contains("Tag")) && this.listTag.Count() == 0)
                            {
                                user_note_backup.Username = Username;
                                user_note_backup.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                                note_backup.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                                note_backup.Header = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Header;
                                note_backup.Context = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Context;
                                note_backup.Time = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].Time;
                                note_backup.TimeEdit = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].TimeEdit;

                                Note_backupcontroller.AddNote_Backup(note_backup);
                                User_Note_backupcontroller.AddUser_Note_Backup(user_note_backup);
                                user_note.ID = this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID;
                                user_note.Username = Username;

                                User_Notecontrollers.DeleteUser_Note(user_note);

                                List<Note_Tag> note_tag_delete = Tag_Notecontroller.GetTag(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1].ID);
                                for (int i = 0; i < note_tag_delete.Count(); i++)
                                {
                                    Tag_Notecontroller.DeleteNote_Tag(note_tag_delete[i]);
                                }

                                Notecontrollers.DeleteNote(this.listNoteSearchHeader[item.Index - this.listTag.Count() - 1]);
                                this.listNoteSearchHeader.RemoveAt(item.Index - this.listTag.Count() - 1);
                                this.lst_Note.Items.Remove(item);
                            }
                            if (this.txtSearchNotes.Text.Contains("Tag: ") || (!this.txtSearchNotes.WaterMark.Contains("All Notes") && !this.txtSearchNotes.WaterMark.Contains("Trash")))
                            {
                                user_note_backup.Username = Username;
                                user_note_backup.ID = notetemp[item.Index].ID;
                                note_backup.ID = notetemp[item.Index].ID;
                                note_backup.Header = notetemp[item.Index].Header;
                                note_backup.Context = notetemp[item.Index].Context;
                                note_backup.Time = notetemp[item.Index].Time;
                                note_backup.TimeEdit = notetemp[item.Index].TimeEdit;

                                Note_backupcontroller.AddNote_Backup(note_backup);
                                User_Note_backupcontroller.AddUser_Note_Backup(user_note_backup);
                                user_note.ID = notetemp[item.Index].ID;
                                user_note.Username = Username;

                                User_Notecontrollers.DeleteUser_Note(user_note);

                                List<Note_Tag> note_tag_delete = Tag_Notecontroller.GetTag(notetemp[item.Index].ID);
                                for (int i = 0; i < note_tag_delete.Count(); i++)
                                {
                                    Tag_Notecontroller.DeleteNote_Tag(note_tag_delete[i]);
                                }

                                Notecontrollers.DeleteNote(notetemp[item.Index]);
                                this.notetemp.RemoveAt(item.Index);
                                this.lst_Note.Items.Remove(item);
                            }
                        }
                    }
                    this.txt_Header.Clear();
                    this.rtb_Note.Clear();

                    this.pnAddTag.Controls.Clear();
                    cButton = 3;
                    this.txtAddTag.Clear();
                    this.pnAddTag.Controls.Add(this.txtAddTag);
                    this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                    this.txtAddTag.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Mời chọn note để xóa", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void txtSearchNotes_TextChanged(object sender, EventArgs e)
        {
            this.lst_Note.Clear();
            this.txtSearchNotes.WaterMark = "All Notes";
            if (!(this.txtSearchNotes.Text.Contains("Tag:")))
            {

                this.listNoteSearchHeader.Clear();
                this.listNoteSearchTag.Clear();
                this.listTag.Clear();
                listNoteSearchHeader = Notecontrollers.getListNote_searchHeader(this.txtSearchNotes.Text.Trim());
                listNoteSearchTag = Tag_Notecontroller.getListNote_searchTag(this.txtSearchNotes.Text.Trim());
                if (listNoteSearchHeader.Count() > 0 || listNoteSearchTag.Count() > 0)
                {
                    checkSearch = true;
                }
                if (listNoteSearchHeader.Count() == 0 && listNoteSearchTag.Count == 0)
                {
                    return;
                }

                if (listNoteSearchTag.Count() > 0)
                {
                    this.lst_Note.Items.Add("Search by Tag: ");
                    string t = null;
                    for (int i = 0; i < listNoteSearchTag.Count(); i++)
                    {
                        t = listNoteSearchTag[i].MiniTag;
                        bool check = false;
                        for (int j = 0; j < listTag.Count(); j++)
                        {
                            if (t == listTag[j])
                            {
                                check = true;
                            }
                        }
                        if (check == false)
                        {
                            listTag.Add(t);
                            this.lst_Note.Items.Add("Tag: " + t);
                        }
                    }
                }

                if (listNoteSearchHeader.Count() > 0)
                {
                    this.lst_Note.Items.Add("Notes: ");
                    for (int i = 0; i < listNoteSearchHeader.Count(); i++)
                    {
                        User_Note user_note_temp = new User_Note();
                        user_note_temp.ID = listNoteSearchHeader[i].ID;
                        user_note_temp.Username = Username;
                        if (User_Notecontrollers.checkUser_Note(user_note_temp) == true)
                        {
                            this.lst_Note.Items.Add(listNoteSearchHeader[i].Header);
                        }
                       else
                        {
                            this.listNoteSearchHeader.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                }
            }
            if (this.txtSearchNotes.Text.Contains("Tag:"))
            {
                this.note_tag.Clear();
                this.notetemp.Clear();
                string temp = null;
                for (int k = 5; k < this.txtSearchNotes.Text.Length; k++)
                {
                    temp += this.txtSearchNotes.Text[k];
                }
                note_tag = Tag_Notecontroller.LoadNoteofTag(temp);
                this.lst_Note.Clear();

                for (int i = 0; i < note_tag.Count(); i++)
                {
                    notetemp.Add(Notecontrollers.LoadNote(note_tag[i].ID));
                }
                for (int j = 0; j < notetemp.Count(); j++)
                {
                    User_Note user_note_temp = new User_Note();
                    user_note_temp.ID = notetemp[j].ID;
                    user_note_temp.Username = Username;
                    if (User_Notecontrollers.checkUser_Note(user_note_temp)==true)
                    {
                        this.lst_Note.Items.Add(notetemp[j].Header);
                    }
                    else
                    {
                        this.notetemp.RemoveAt(j);
                        j--;
                    }   
                }
            }
            if (this.txtSearchNotes.Text.Trim().Count() == 0)
            {
                this.lst_Note.Clear();
                this.listTag.Clear();
                this.listNoteSearchHeader.Clear();
                this.listNoteSearchTag.Clear();
                this.note_tag.Clear();
                this.notetemp.Clear();

                this.pnAddTag.Controls.Clear();
                cButton = 3;
                this.txt_Header.Clear();
                this.rtb_Note.Clear();
                this.txtAddTag.Clear();
                this.pnAddTag.Controls.Add(this.txtAddTag);
                this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                this.txtAddTag.Visible = true;
                this.ListNotes.Clear();
                this.ListUser_Notes.Clear();

                checkSearch = false;
                ListUser_Notes = User_Notecontrollers.LoadNoteofUser(Username);
                for (int i = 0; i < this.ListUser_Notes.Count(); i++)
                {
                    this.ListNotes.Add(Notecontrollers.LoadNote(this.ListUser_Notes[i].ID));
                }
                for (int j = 0; j < this.ListNotes.Count(); j++)
                {
                    this.lst_Note.Items.Add(this.ListNotes[j].Header);
                }            
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (t < 25)
            {
                this.rtb_Note.Font = new Font("Time New Roman", t, GraphicsUnit.Point);
                t += 1;
            }
            else
            {
                return;
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (t > 9)
            {
                this.rtb_Note.Font = new Font("Time New Roman", t, GraphicsUnit.Point);
                t -= 1;
            }
            else
            {
                return;
            }
        }

        private void actualSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t = 15;
            this.rtb_Note.Font = new Font("Time New Roman", t, GraphicsUnit.Point);
        }

        private void tsmSearchNotes_Click(object sender, EventArgs e)
        {
            this.txtSearchNotes.Focus();
        }
        private void dateCreatedToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (dateCreatedToolStripMenuItem.Checked)
            {
                alphabeticalToolStripMenuItem.Checked = false;
            }
        }

        private void alphabeticalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (alphabeticalToolStripMenuItem.Checked)
            {
                dateCreatedToolStripMenuItem.Checked = false;
            }
        }

        private void fullToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

            if (fullToolStripMenuItem.Checked)
            {
                narrowToolStripMenuItem.Checked = false;
            }
        }

        private void narrowToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (narrowToolStripMenuItem.Checked)
            {
                fullToolStripMenuItem.Checked = false;
            }
        }

        private void fullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fullToolStripMenuItem.Checked)
            {
                this.rtb_Note.SelectionIndent = 0;
            }
        }

        private void narrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (narrowToolStripMenuItem.Checked)
            {
                if (lst_Note.SelectedItems.Count > 0)
                {
                    this.rtb_Note.SelectionIndent = 50;
                }
            }
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            if (darkToolStripMenuItem.Checked)
            {
                lightToolStripMenuItem.Checked = false;
                darkToolStripMenuItem.CheckState = CheckState.Checked;
                this.BackColor = Color.FromArgb(36, 37, 38);
                this.btnMenu.BaseColor = Color.Gray;
                this.btnInfo.BaseColor = Color.Gray;
                this.btnAllNotes.BaseColor = Color.Gray;
                this.btnTrash.BaseColor = Color.Gray;
                this.btnDelete.BaseColor = Color.Gray;
                this.btnNewNote.BaseColor = Color.Gray;
                this.lst_Note.BackColor = Color.FromArgb(36, 37, 38);
                this.lst_Note.ForeColor = Color.White;
                this.txt_Header.BackColor = Color.FromArgb(36, 37, 38);
                this.txt_Header.ForeColor = Color.White;
                this.rtb_Note.BackColor = Color.FromArgb(36, 37, 38);
                this.rtb_Note.ForeColor = Color.White;
                this.txtAddTag.BackColor = Color.FromArgb(36, 37, 38);
                this.txtAddTag.ForeColor = Color.White;
                this.txtAddTag.WaterMarkForeColor = Color.White;
                this.btnAllNotes.OnHoverBaseColor = Color.FromArgb(36, 37, 38);
                this.btnTrash.OnHoverBaseColor = Color.FromArgb(36, 37, 38);
                this.btnExit.BaseColor = Color.Gray;
                this.gunaLabel1.ForeColor = Color.White;
                this.gunaLabel2.ForeColor = Color.White;
                this.lblShowDateCreated.ForeColor = Color.White;
                this.lblDateCreated.ForeColor = Color.White;
                this.lblCountCharacter.ForeColor = Color.White;
                this.lblCountWords.ForeColor = Color.White;
                this.lblShowDate.ForeColor = Color.White;
                this.ptBOpacity.BaseColor = Color.FromArgb(36, 37, 38);
                this.txtSearchNotes.ForeColor = Color.White;
                this.txtSearchNotes.WaterMarkForeColor = Color.White;
                this.txtSearchNotes.BackColor = Color.FromArgb(36, 37, 38);
                this.btnDeleteForever.BaseColor = Color.FromArgb(36, 37, 38);
                this.btnDeleteForever.OnHoverBaseColor = Color.FromArgb(36, 37, 38);
                this.btnMenu.BorderColor = Color.White;
                this.btnInfo.BorderColor = Color.White;
                this.btnAllNotes.BorderColor = Color.White;
                this.btnTrash.BorderColor = Color.White;
                this.btnDelete.BorderColor = Color.White;
                this.btnNewNote.BorderColor = Color.White;
                this.btnExit.BorderColor = Color.White;

                this.lblTags.ForeColor = Color.White;
                this.lblTags.BackColor = Color.FromArgb(36, 37, 38);
                this.lst_Tag.BackColor = Color.FromArgb(36, 37, 38);
                this.lst_Tag.ForeColor = Color.White;
                this.pnEdit.BackColor = Color.FromArgb(36, 37, 38);
                this.btnDeleteTag.BaseColor = Color.FromArgb(36, 37, 38);
            }
            else
            {
                darkToolStripMenuItem.Checked = true;
            }
        }

        private void focusModeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (flaq == true)
            {
                this.txtSearchNotes.Visible = false;
                this.btnNewNote.Visible = false;
                this.lst_Note.Visible = false;
                this.btnMenu.Visible = false;
                this.pnFocus.Location = new Point(0, 63);
                this.pnFocus.Size = new Size(995, 518);
                this.pnAddTag.Width = this.pnFocus.Width;
                this.rtb_Note.Width = this.pnFocus.Width;
                this.txt_Header.Width = this.pnFocus.Width;
                this.swtToggle.Checked = false;
                if (this.lst_Note.SelectedItems.Count > 0)
                {
                    this.pnAddTag.Visible = true;
                }
                else
                    this.pnAddTag.Visible = false;
                flaq = false;
            }
            else
            {
                this.swtToggle.Checked = true;
                this.txtSearchNotes.Visible = true;
                this.btnNewNote.Visible = true;
                this.lst_Note.Visible = true;
                this.btnMenu.Visible = true;
                this.pnFocus.Location = new Point(332, 63);
                this.pnFocus.Size = new Size(663, 518);
                if (this.lst_Note.SelectedItems.Count > 0)
                {
                    this.pnAddTag.Visible = true;
                }
                else
                    this.pnAddTag.Visible = false;
                flaq = true;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (flaqMenu == true)
            {
                ShowMenu();
                flaqMenu = false;
            }
            else
            {
                HideMenu();
                flaqMenu = true;
            }
            if(this.sortAlphabeticalyToolStripMenuItem.Checked)
            {
                this.lst_Tag.Clear();
                this.lst_Tag.Visible = true;
                List<Tag> tag = new List<Tag>();
                tag = Tagcontroller.LoadTag();
                for (int i = 0; i < tag.Count(); i++)
                {
                    this.lst_Tag.Items.Add(tag[i].Tag1);
                }
            }
            else
            {
                this.lst_Tag.Clear();
                for (int i = 0; i < TagSort.Count(); i++)
                {
                    this.lst_Tag.Items.Add(TagSort[i].Tag1);
                }         
            }     
        }

        private void pnAll_Click(object sender, EventArgs e)
        {
            HideMenu();
            flaqMenu = true;
        }

        private void btnAllNotes_Click(object sender, EventArgs e)
        {
            if (flaq1 == true)
            {
                
                this.btnDelete.Visible = false;
                this.btnInfo.Visible = false;
                this.btnDeleteForever.Visible = false;
                this.btnRestoreNote.Visible = false;
                this.btnAllNotes.ForeColor = Color.Black;
                flaq1 = false;
                HideMenu();
                this.txtSearchNotes.WaterMark = "All Notes";
                this.btnAllNotes.OnHoverForeColor = Color.Black;
                TrashHide();
                this.lst_Note.Clear();
                this.ListUser_Notes.Clear();
                this.ListNotes.Clear();
                this.txtSearchNotes.Text = "";
                ListUser_Notes = User_Notecontrollers.LoadNoteofUser(Username);
                for (int i = 0; i < this.ListUser_Notes.Count(); i++)
                {
                    this.ListNotes.Add(Notecontrollers.LoadNote(this.ListUser_Notes[i].ID));
                }
                for (int j = 0; j < this.ListNotes.Count(); j++)
                {
                    this.lst_Note.Items.Add(this.ListNotes[j].Header);
                }
                flaqMenu = true;
            }
            else
            {
                HideMenu();
                flaq1 = false;
                flaqMenu = true;
                this.txtSearchNotes.Text = "";
            }
            if (flaq2 == false)
            {
                HideMenu();
                this.btnTrash.ForeColor = Color.DarkGray;
                this.btnTrash.OnHoverForeColor = Color.DarkGray;
                this.btnTrash.OnHoverBorderColor = Color.DarkGray;
                flaq2 = true;
            }
        }

        private void btnTrash_Click(object sender, EventArgs e)
        {
            if (flaq2 == true)
            {
                this.txtSearchNotes.Text = "";
                this.txtAddTag.Visible = false;
                this.btnDelete.Visible = false;
                this.btnInfo.Visible = false;
                this.btnTrash.ForeColor = Color.Black;
                flaq2 = false;
                HideMenu();
                this.txtSearchNotes.WaterMark = "Trash";
                this.btnTrash.OnHoverForeColor = Color.Black;
                TrashShow();
                this.lst_Note.Clear();
                this.ListNotes_Backup.Clear();
                this.ListUser_Notes_Backup.Clear();
                ListUser_Notes_Backup = User_Note_backupcontroller.LoadNoteofUser_Backup(Username);
                for (int i = 0; i < this.ListUser_Notes_Backup.Count(); i++)
                {
                    this.ListNotes_Backup.Add(Note_backupcontroller.LoadNote_Backup(this.ListUser_Notes_Backup[i].ID));
                }
                for (int j = 0; j < this.ListNotes_Backup.Count(); j++)
                {
                    this.lst_Note.Items.Add(this.ListNotes_Backup[j].Header);
                }
                flaqMenu = true;
            }
            else
            {
                HideMenu();
                flaq = false;
                flaqMenu = true;
            }
            if (flaq1 == false)
            {
                this.btnAllNotes.ForeColor = Color.DarkGray;
                this.btnAllNotes.OnHoverForeColor = Color.DarkGray;
                this.btnAllNotes.OnHoverBorderColor = Color.DarkGray;
                flaq1 = true;
            }
        }

        private void swtToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (swtToggle.Checked)
            {
                this.txtSearchNotes.Visible = true;
                this.btnNewNote.Visible = true;
                this.lst_Note.Visible = true;
                this.btnMenu.Visible = true;
                this.pnFocus.Location = new Point(332, 63);
                this.pnFocus.Size = new Size(663, 518);
                this.swtToggle.Location = new Point(335, 28);

                if (this.lst_Note.SelectedItems.Count > 0)
                {
                    this.pnAddTag.Visible = true;
                }
                else
                    this.pnAddTag.Visible = false;
                flaq = true;
            }
            else
            {
                this.txtSearchNotes.Visible = false;
                this.btnNewNote.Visible = false;
                this.lst_Note.Visible = false;
                this.btnMenu.Visible = false;
                this.pnFocus.Location = new Point(0, 63);
                this.pnFocus.Size = new Size(995, 518);
                this.pnAddTag.Width = this.pnFocus.Width;
                this.rtb_Note.Width = this.pnFocus.Width;
                this.txt_Header.Width = this.pnFocus.Width;
                this.swtToggle.Location = new Point(16, 28);
                if (this.lst_Note.SelectedItems.Count > 0)
                {
                    this.pnAddTag.Visible = true;
                }
                else
                    this.pnAddTag.Visible = false;
                flaq = false;
            }
        }

        private void ptBOpacity_Click(object sender, EventArgs e)
        {
            if (flaqMenu == false)
            {
                if(lst_Note.SelectedItems.Count > 0)
                {
                    HideMenu();
                    this.btnFont.Visible = true;
                    this.btnColor.Visible = true;
                    if(this.txtSearchNotes.WaterMark == "Trash")
                    {
                        this.btnFont.Visible = false;
                        this.btnColor.Visible = false;
                    }
                    flaqMenu = true;
                }
                else
                {
                    HideMenu();
                    flaqMenu = true;
                }
            }
            if (flaqInfo == false)
            {
                HideInfo();
                flaqInfo = true;
            }        
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if(flaqInfo == true)
            {
                ShowInfo();
                flaqInfo = false;
            }
            else
            {
                HideInfo();
                flaqInfo = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            HideInfo();
            flaqInfo = true;
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lightToolStripMenuItem.Checked)
            {
                this.BackColor = Color.FromArgb(255, 255, 192);
                this.btnMenu.BaseColor = Color.FromArgb(255, 255, 192);
                this.btnInfo.BaseColor = Color.FromArgb(255, 255, 192);
                this.btnAllNotes.BaseColor = Color.FromArgb(255, 255, 192);
                this.btnTrash.BaseColor = Color.FromArgb(255, 255, 192);
                this.btnDelete.BaseColor = Color.FromArgb(255, 255, 192);
                this.btnNewNote.BaseColor = Color.FromArgb(255, 255, 192);
                this.lst_Note.BackColor = Color.FromArgb(255, 255, 192);
                this.lst_Note.ForeColor = Color.Black;
                this.txt_Header.BackColor = Color.FromArgb(255, 255, 192);
                this.txt_Header.ForeColor = Color.Black;
                this.rtb_Note.BackColor = Color.FromArgb(255, 255, 192);
                this.rtb_Note.ForeColor = Color.Black;
                this.txtAddTag.BackColor = Color.FromArgb(255, 255, 192);
                this.txtAddTag.ForeColor = Color.Black;
                this.btnExit.BaseColor = Color.FromArgb(255, 255, 192);
                this.gunaLabel1.ForeColor = Color.Black;
                this.gunaLabel2.ForeColor = Color.Black;
                this.lblCountCharacter.ForeColor = Color.Black;
                this.lblCountWords.ForeColor = Color.Black;
                this.lblShowDate.ForeColor = Color.Black;
                this.lblShowDateCreated.ForeColor = Color.Black;
                this.lblDateCreated.ForeColor = Color.Black;
                darkToolStripMenuItem.Checked = false;
                this.ptBOpacity.BaseColor = Color.White;
                this.txtSearchNotes.ForeColor = Color.Black;
                this.txtSearchNotes.WaterMarkForeColor = Color.Black;
                this.txtSearchNotes.BackColor = Color.AliceBlue;
                this.btnAllNotes.OnHoverBaseColor = Color.FromArgb(255, 255, 192);
                this.btnTrash.OnHoverBaseColor = Color.FromArgb(255, 255, 192);
                this.btnMenu.BorderColor = Color.Black;
                this.btnInfo.BorderColor = Color.Black;
                this.btnAllNotes.BorderColor = Color.Black;
                this.btnTrash.BorderColor = Color.Black;
                this.btnDelete.BorderColor = Color.Black;
                this.btnNewNote.BorderColor = Color.Black;
                this.btnExit.BorderColor = Color.Black;

                this.lblTags.ForeColor = Color.Black;
                this.lblTags.BackColor = Color.FromArgb(255, 251, 209);
                this.lst_Tag.BackColor = Color.FromArgb(255, 251, 209);
                this.lst_Tag.ForeColor = Color.Black;
                this.pnEdit.BackColor = Color.FromArgb(255, 251, 209);
                this.btnDeleteTag.BaseColor = Color.FromArgb(255, 251, 209);
            }
            else
            {
                lightToolStripMenuItem.Checked = true;
            }
        }

        private void tsmExportNote_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save File";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt| All Files (*.*)|*.*";
            saveFileDialog.FileName = text_header_save + ".txt";
            if (this.lst_Note.SelectedItems.Count > 0)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter write = new StreamWriter(File.Create(saveFileDialog.FileName));
                    write.Write(text_save);
                    write.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Mời chọn note để lưu", "Thông báo", MessageBoxButtons.OK);
            }

        }

        private void tsmPrint_Click(object sender, EventArgs e)
        {
            if (this.lst_Note.SelectedItems.Count > 0)
            {
                if (printDialog.ShowDialog() == DialogResult.OK)
                {

                        printDocument.Print();                 

                }
            }
            else
            {
                MessageBox.Show("Mời chọn note để in", "Thông báo", MessageBoxButtons.OK);
            }
            
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(text_print, new Font("Times New Roman", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void tsmImportNote_Click(object sender, EventArgs e)
        {
            if (this.txtSearchNotes.WaterMark.Contains("All Notes") && checkSearch == false)
            {
                Note notetemp = new Note();
                User_Note user_notetemp = new User_Note();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Text Files (*.txt)|*.txt| All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader read = new StreamReader(File.OpenRead(openFileDialog.FileName));
                    string name = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    notetemp.Header = name;
                    notetemp.ID = ID;
                    ID++;
                    notetemp.Context = read.ReadToEnd();
                    notetemp.Time = DateTime.Now.ToString(format);
                    notetemp.TimeEdit = DateTime.Now.ToString(format);
                    user_notetemp.Username = Username;
                    user_notetemp.ID = notetemp.ID;

                    this.ListNotes.Add(notetemp);
                    this.ListUser_Notes.Add(user_notetemp);
                    Notecontrollers.AddNote(notetemp);
                    User_Notecontrollers.AddUser_Note(user_notetemp);

                    this.lst_Note.Clear();
                    this.ListNotes.Clear();
                    this.ListUser_Notes.Clear();

                    ListUser_Notes = User_Notecontrollers.LoadNoteofUser(Username);
                    for (int i = 0; i < this.ListUser_Notes.Count(); i++)
                    {
                        this.ListNotes.Add(Notecontrollers.LoadNote(this.ListUser_Notes[i].ID));
                    }
                    for (int j = 0; j < this.ListNotes.Count(); j++)
                    {
                        this.lst_Note.Items.Add(this.ListNotes[j].Header);
                    }

                    this.txt_Header.Text = name;
                    this.rtb_Note.Text = read.ReadToEnd();
                    read.Dispose();
                }
            }
        }

        private void btnDeleteForever_Click(object sender, EventArgs e)
        {
            if (this.lst_Note.SelectedItems.Count > 0)
            {
                if (this.txtSearchNotes.WaterMark.Contains("Trash") && checkSearch == false)
                {

                    foreach (ListViewItem item in this.lst_Note.SelectedItems)
                    {
                        User_Note_Backup user_note_backup = new User_Note_Backup();

                        user_note_backup.ID = ListNotes_Backup[item.Index].ID;
                        user_note_backup.Username = Username;

                        User_Note_backupcontroller.DeleteUser_Note_Backup(user_note_backup);
                        Note_backupcontroller.DeleteNote_Backup(this.ListNotes_Backup[item.Index]);

                        this.ListNotes_Backup.RemoveAt(item.Index);
                        this.lst_Note.Items.Remove(item);
                        this.pnAddTag.Visible = false;
                        this.txt_Header.Clear();
                        this.rtb_Note.Clear();

                        this.pnAddTag.Controls.Clear();
                        cButton = 3;
                        this.txtAddTag.Clear();
                        this.pnAddTag.Controls.Add(this.txtAddTag);
                        this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                        this.txtAddTag.Visible = true;
                    }
                }
            }
        }

        private void btnRestoreNote_Click(object sender, EventArgs e)
        {
            if (this.lst_Note.SelectedItems.Count > 0)
            {
                if (this.txtSearchNotes.WaterMark.Contains("Trash") && checkSearch == false)
                {

                    foreach (ListViewItem item in this.lst_Note.SelectedItems)
                    {
                        User_Note user_note_temp = new User_Note();
                        Note notetemp = new Note();
                        this.pnAddTag.Visible = false;
                        notetemp.ID = ID;
                        ID++;
                        notetemp.Header = this.ListNotes_Backup[item.Index].Header;
                        notetemp.Context = this.ListNotes_Backup[item.Index].Context;
                        notetemp.Time = this.ListNotes_Backup[item.Index].Time;
                        notetemp.TimeEdit = this.ListNotes_Backup[item.Index].TimeEdit;

                        user_note_temp.ID = notetemp.ID;
                        user_note_temp.Username = Username;

                        Notecontrollers.AddNote(notetemp);
                        User_Notecontrollers.AddUser_Note(user_note_temp);

                        User_Note_Backup user_note_backup = new User_Note_Backup();

                        user_note_backup.ID = ListNotes_Backup[item.Index].ID;
                        user_note_backup.Username = Username;

                        User_Note_backupcontroller.DeleteUser_Note_Backup(user_note_backup);
                        Note_backupcontroller.DeleteNote_Backup(this.ListNotes_Backup[item.Index]);

                        this.ListNotes_Backup.RemoveAt(item.Index);
                        this.lst_Note.Items.Remove(item);

                        this.txt_Header.Clear();
                        this.rtb_Note.Clear();

                        this.pnAddTag.Controls.Clear();
                        cButton = 3;
                        this.txtAddTag.Clear();
                        this.pnAddTag.Controls.Add(this.txtAddTag);
                        this.txtAddTag.Location = new System.Drawing.Point(0, -3);
                        this.txtAddTag.Visible = true;

                    }
                }
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lst_Tag.SelectedItems)
            {
                Tag tag = new Tag();
                List<Tag> tagtemp = new List<Tag>();
                tagtemp = Tagcontroller.LoadTag();
                tag = tagtemp[item.Index];
                List<Note_Tag> note_tag_temp = new List<Note_Tag>();
                note_tag_temp = Tag_Notecontroller.getListNote_searchTag(tag.Tag1);
                for (int i = 0; i < note_tag_temp.Count(); i++)
                {
                    Tag_Notecontroller.DeleteNote_Tag(note_tag_temp[i]);
                }
                Tagcontroller.DeleteTag(tag);
                lst_Tag.Items.Remove(item);

                this.lst_Tag.Clear();
                this.lst_Tag.Visible = true;
                tagtemp = Tagcontroller.LoadTag();
                for (int i = 0; i < tagtemp.Count(); i++)
                {
                    this.lst_Tag.Items.Add(tagtemp[i].Tag1);
                }
            }
        }

        private void lst_Tag_Click(object sender, EventArgs e)
        {
            if(this.lblEdit.Text == "Edit")
            {
                foreach(ListViewItem item in lst_Tag.SelectedItems)
                {

                    this.btnDeleteForever.Visible = false;
                    this.btnRestoreNote.Visible = false;
                    this.txtSearchNotes.WaterMark = item.Text;
                    this.note_tag.Clear();
                    this.notetemp.Clear();
                    note_tag = Tag_Notecontroller.getListNote_searchTag(this.txtSearchNotes.WaterMark.Trim());
                    this.lst_Note.Clear();

                    for (int i = 0; i < note_tag.Count(); i++)
                    {
                        notetemp.Add(Notecontrollers.LoadNote(note_tag[i].ID));
                    }
                    for (int j = 0; j < notetemp.Count(); j++)
                    {
                        User_Note user_note_temp = new User_Note();
                        user_note_temp.ID = notetemp[j].ID;
                        user_note_temp.Username = Username;
                        if (User_Notecontrollers.checkUser_Note(user_note_temp) == true)
                        {
                            this.lst_Note.Items.Add(notetemp[j].Header);
                        }
                        else
                        {
                            this.notetemp.RemoveAt(j);
                            j--;
                        }

                    }
                    this.txt_Header.Visible = false;
                    this.rtb_Note.Visible = false;
                    this.pnAddTag.Visible = false;
                    HideMenu();
                    flaqMenu = true;
                    flaq1 = true;
                }
            }         
        }

        private void lblEdit_Click(object sender, EventArgs e)
        {
            if(checkEdit == true)
            {
                this.lblEdit.Text = "Done";
                this.btnDeleteTag.Visible = true;
                checkEdit = false;
            }
            else
            {
                this.lblEdit.Text = "Edit";
                this.btnDeleteTag.Visible = false;
                checkEdit = true;
            }
        }

        private void lblEdit_MouseHover(object sender, EventArgs e)
        {
            this.lblEdit.Cursor = Cursors.Hand;
        }

        private void lblEdit_MouseLeave(object sender, EventArgs e)
        {
            this.lblEdit.Cursor = Cursors.Default;
        }

        private void dateCreatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkDateCreated == true)
            {
                var dateD = (from u in ListNotes
                            orderby u.Time descending
                            select u).ToList();
                this.lst_Note.Clear();
                this.ListNotes.Clear();
                for(int i= 0;i<dateD.Count();i++)
                {
                    ListNotes.Add(dateD[i]);
                    lst_Note.Items.Add(dateD[i].Header);
                }
                this.dateCreatedToolStripMenuItem.Checked = true;
                checkDateCreated = false;
            }
            else
            {
                var dateA = (from u in ListNotes
                            orderby u.Time ascending
                            select u).ToList();
                this.lst_Note.Clear();
                ListNotes.Clear();
                for (int i = 0; i < dateA.Count(); i++)
                {
                    ListNotes.Add(dateA[i]);
                    lst_Note.Items.Add(dateA[i].Header);
                }
                this.dateCreatedToolStripMenuItem.Checked = true;
                checkDateCreated = true;
            }
        }

        private void alphabeticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkAlphabeticalHeader == true)
            {
                var headerD = (from u in ListNotes
                              orderby u.Header descending
                              select u).ToList();
                this.lst_Note.Clear();
                ListNotes.Clear();
                for (int i = 0; i < headerD.Count(); i++)
                {
                    ListNotes.Add(headerD[i]);
                    lst_Note.Items.Add(headerD[i].Header);
                }
                this.alphabeticalToolStripMenuItem.Checked = true;
                checkAlphabeticalHeader = false;
            }
            else
            {
                var headerA = (from u in ListNotes
                              orderby u.Header ascending
                              select u).ToList();
                this.lst_Note.Clear();
                ListNotes.Clear();
                for (int i = 0; i < headerA.Count(); i++)
                {
                    ListNotes.Add(headerA[i]);
                    lst_Note.Items.Add(headerA[i].Header);
                }
                this.alphabeticalToolStripMenuItem.Checked = true;
                checkAlphabeticalHeader = true;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog myColor = new ColorDialog();
            if (myColor.ShowDialog() == DialogResult.OK)
            {
                this.rtb_Note.SelectionColor = myColor.Color;
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.rtb_Note.SelectionFont = fd.Font;
            }
        }

        private void reversedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkReversed == true)
            {
                var reversedT = (from u in ListNotes
                             select u).ToList();
                this.lst_Note.Clear();
                this.ListNotes.Clear();
                for (int i = reversedT.Count()-1; i >= 0; i--)
                {
                    ListNotes.Add(reversedT[i]);
                    lst_Note.Items.Add(reversedT[i].Header);
                }
                checkReversed = false;
            }
            else
            {
                var reversedF = (from u in ListNotes
                                 select u).ToList();
                this.lst_Note.Clear();
                this.ListNotes.Clear();
                for (int i = reversedF.Count() - 1; i >= 0; i--)
                {
                    ListNotes.Add(reversedF[i]);
                    lst_Note.Items.Add(reversedF[i].Header);
                }
                checkReversed = true;
            }
        }
        private void sortAlphabeticalyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TagSort = Tagcontroller.LoadTag();
            if (checkTagSort == true)
            {
                var TagD = (from u in TagSort
                            orderby u.Tag1 descending
                            select u).ToList();
                this.lst_Tag.Clear();
                TagSort.Clear();
                for (int i = 0; i < TagD.Count(); i++)
                {
                    TagSort.Add(TagD[i]);
                    this.lst_Tag.Items.Add(TagD[i].Tag1);
                }
                this.sortAlphabeticalyToolStripMenuItem.Checked = false;
                checkTagSort = false;
            }
            else
            {
                var TagA = (from u in TagSort
                            orderby u.Tag1 ascending
                            select u).ToList();
                this.lst_Tag.Clear();
                TagSort.Clear();
                for (int i = 0; i < TagA.Count(); i++)
                {
                    TagSort.Add(TagA[i]);
                    this.lst_Tag.Items.Add(TagA[i].Tag1);
                }
                this.sortAlphabeticalyToolStripMenuItem.Checked = true;
                checkTagSort = true;
            }
        }
        #endregion

    }
}

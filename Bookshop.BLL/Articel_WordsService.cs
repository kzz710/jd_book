using Bookshop.Common;
using Bookshop.IBLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Bookshop.BLL
{
    public class Articel_WordsService:BaseService<Articel_Words>,IArticel_WordsService
    {

        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.Articel_WordsDal;
        }



        public bool CheckForbiddenWord(string Msg)
        {
            //得到所有的禁用词
            List<string> newList = new List<string>();
            if (MemcacheHelper.Get("forbiddenWords") == null)
            {
                List<Articel_Words> list = this.CurrentDBSession.Articel_WordsDal.LoadEntities(w => w.IsForbid == true).ToList();
                foreach (Articel_Words word in list)
                {
                    newList.Add(word.WordPattern);
                }
                MemcacheHelper.Set("forbiddenWords", newList);

            }
            else 
            {
                newList = (List<string>)MemcacheHelper.Get("forbiddenWords");
            }
            //匹配用户输入的评论时候是否有禁用词
            string regex = string.Join("|",newList.ToArray());
            regex = regex.Replace(@"\",@"\\").Replace("{2}",@".{0,2}");
            return Regex.IsMatch(Msg,regex);
        }




        public bool CheckModWord(string Msg)
        {
            //得到所有的审查词
            List<string> newList = new List<string>();
            if (MemcacheHelper.Get("modWords")==null)
            {
                List<Articel_Words> list = this.CurrentDBSession.Articel_WordsDal.LoadEntities(w=>w.IsMod==true).ToList();
                foreach (Articel_Words word in list)
                {
                    newList.Add(word.WordPattern);
                }
                MemcacheHelper.Set("modWords",newList);
            }
            else
            {
                newList = (List<string>)MemcacheHelper.Get("modWords");
            }
            //匹配用户输入的评论时候是否有审查词
            string regex = string.Join("|",newList.ToArray());
            regex = regex.Replace(@"\", @"\\").Replace("{2}", @".{0,2}");
            return Regex.IsMatch(Msg, regex);
        }

        public string CheckReplaceWord(string Msg)
        {
            //得到替换词
            List<Articel_Words> list=new List<Articel_Words>();
            if (MemcacheHelper.Get("replaceWords")==null)
            {
                list = this.CurrentDBSession.Articel_WordsDal.LoadEntities(w=>w.IsMod==false&&w.IsForbid==false).ToList();
                MemcacheHelper.Set("replaceWords",SerializeHelper.SerializeToString(list));
            }
            else
            {
                list = SerializeHelper.DeserializeToObject<List<Articel_Words>>(MemcacheHelper.Get("replaceWords").ToString());
            }
            foreach (Articel_Words word in list)
            {
                Msg = Msg.Replace(word.WordPattern,word.ReplaceWord);
            }
            return Msg;
        }
    }
}

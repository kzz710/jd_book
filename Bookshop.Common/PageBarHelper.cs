using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShop.Common
{
    public class PageBarHelper
    {
        public static string GetBar(int pageIndex,int pageCount) 
        {
            if (pageCount==1)
            {
                return string.Empty;
            }
            else
            {
                int start = pageIndex - 5;
                if (start<1)
                {
                    start = 1;
                }
                int end = start + 9;
                if (end>pageCount)
                {
                    end = pageCount;
                    //重新计算start值
                    start = end - 9<1 ? 1 : end - 9;
                }
                StringBuilder sb = new StringBuilder();
                if (pageIndex>1)
                {
                    //href后面不写文件名就表示当前页面
                    sb.AppendFormat("<a href='?pageIndex={0}' class='page'>上一页</a>",pageIndex-1);
                }
                for (int i = start; i <=end; i++)
                {
                    if (i==pageIndex)
                    {
                        sb.Append(i);
                    }
                    else
                    {
                        sb.AppendFormat("<a href='?pageIndex={0}' class='page'>{0}</a>",i);
                    }
                }
                if (pageIndex<pageCount)
                {
                    sb.AppendFormat("<a href='?pageIndex={0}' class='page'>下一页</a>",pageIndex+1);
                }
                return sb.ToString();
            }
        }
    }
}

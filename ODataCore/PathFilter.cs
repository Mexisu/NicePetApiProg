using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nicepet_API
{
    public class PathFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
            string str = context.HttpContext.Request.QueryString.Value;
            string queryStr = "";
            if (str.Contains("$filter=") && str.Contains("%20in%20"))
            {
                string[] queries = str.Split("&");
                str = queries.ToList().Find(a => a.Contains("%20in%20"));
                string[] andProperties = str.Split("$filter=")[1].Split("%20and%20");
                string[] orProperties = str.Split("$filter=")[1].Split("%20or%20");

                string[] attributes = null;
                string property = null;
                string s = null;
                if (andProperties.Length > 1)
                {
                    s = andProperties.ToList().Find(a => a.Contains("%20in%20"));
                }

                if (orProperties.Length > 1 && s == null)
                {
                    s = orProperties.ToList().Find(a => a.Contains("%20in%20"));
                }

                if (s == null)
                {
                    attributes = str.Split("(")[1].Split(")")[0].Split(",");
                    property = str.Split("%20in%20")[0].Split("$filter=")[1];
                }

                if (s != null)
                {
                    property = s.Split("%20in%20")[0];
                    attributes = s.Split("(")[1].Split(")")[0].Split(",");
                }


                queries.ToList().ForEach(a =>
                {
                    if (a.Contains("%20in%20"))
                    {
                        if (a.Contains("?"))
                        {
                            queryStr += "?";
                        }
                        else
                        {
                            queryStr += "&";
                        }
                        queryStr += "$filter=(" + property + "%20eq%20" + attributes[0];
                        for (int i = 1; i < attributes.Length; i++)
                        {
                            queryStr += "%20or%20" + property + "%20eq%20" + attributes[i];
                        }
                        queryStr += ")";
                        andProperties.ToList().ForEach(b => {
                            if (!b.Contains("%20in%20"))
                                queryStr += "%20and%20(" + b + ")";
                        });
                        orProperties.ToList().ForEach(b => {
                            if (!b.Contains("%20in%20"))
                                queryStr += "%20or%20(" + b + ")";
                        });
                    }
                    else
                    {
                        if (a.Contains("?"))
                        {
                            queryStr += a;
                        }
                        else
                        {
                            queryStr += "&" + a;
                        }
                    }
                });
                context.HttpContext.Request.QueryString = new Microsoft.AspNetCore.Http.QueryString(queryStr);

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
        }
    }
}

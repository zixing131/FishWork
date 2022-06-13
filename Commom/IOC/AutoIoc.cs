using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FishWork.Commom.IOC
{
    /// <summary>
    /// 自动注入类
    /// </summary>
    public class AutoIoc
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        private Dictionary<string, object> dictCache = null;

        static AutoIoc _this = null;
        private AutoIoc() {
            dictCache = new Dictionary<string, object>();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static AutoIoc GetInstance() {
            if (_this == null) {
                _this = new AutoIoc();
            }
            return _this;
        }


        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Register(object obj) {
            dictCache[obj.GetType().ToString()] = obj;
        }

        /// <summary>
        /// 注入属性
        /// </summary>
        /// <param name="obj"></param>
        public void InjectionAttribute(object obj) {
            var perpertys = obj.GetType().GetProperties(BindingFlags.Instance |BindingFlags.NonPublic);
            foreach (var perty in perpertys){
                var attr = perty.GetCustomAttributes(true);
                if (attr != null) {
                    foreach (var item in attr){
                        if (item.GetType().Equals(typeof(InjectionAttribute))) {
                            string key = perty.PropertyType.ToString();
                            if (dictCache.ContainsKey(key)) {
                                perty.SetValue(obj, dictCache[key], null);
                            }
                        }
                    }
                }
            }
        }


    }
}

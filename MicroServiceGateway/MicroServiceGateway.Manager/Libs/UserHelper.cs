﻿/****************************************************************************
*项目名称：MicroServiceGateway.Manager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：MicroServiceGateway.Manager.Libs
*类 名 称：UserHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/8/21 10:48:05
*描述：
*=====================================================================
*修改时间：2020/8/21 10:48:05
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MicroServiceGateway.Model;
using SAEA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MicroServiceGateway.Manager.Libs
{
    /// <summary>
    /// 配置管理
    /// </summary>
    static class UserHelper
    {
        static List<User> _list = new List<User>();


        public static string GetCurrentPath(string children)
        {
            var path = Path.Combine(Path.GetDirectoryName(AssemblyHelper.Current.Location), children);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 添加或更新配置
        /// </summary>
        /// <param name="user"></param>
        public static void Set(User user)
        {
            var old = _list.Where(b => b.ID == user.ID).FirstOrDefault();

            if (old == null)
            {
                _list.Add(user);
            }
            else
            {
                _list.Remove(old);
                _list.Add(user);
            }

            Save();
        }

        public static void Set(List<User> users)
        {
            _list = users;

            Save();
        }

        /// <summary>
        /// 读取配置列表
        /// </summary>
        /// <returns></returns>
        public static List<User> ReadList()
        {
            var filePath = Path.Combine(GetCurrentPath("Config"), "userconfig.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);

                if (!string.IsNullOrEmpty(json))
                {
                    var str = AESHelper.Decrypt(json, "yswenli", false);

                    _list = SerializeHelper.Deserialize<List<User>>(str);

                    if (_list != null && _list.Count > 0)
                        return _list;
                }
            }

            return new List<User>();
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        public static void Save()
        {
            try
            {
                var json = SerializeHelper.Serialize(_list);

                var str = AESHelper.Encrypt(json, "yswenli", false);

                var filePath = Path.Combine(GetCurrentPath("Config"), "userconfig.json");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.AppendAllText(filePath, str);
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserHelper.Save", ex);
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static User Get(string ID)
        {
            if (_list == null || _list.Count < 1) ReadList();

            return _list.Where(b => b.ID == ID).FirstOrDefault();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Login(string userName, string password)
        {
            if (_list == null || !_list.Any()) ReadList();

            if (_list == null || !_list.Any()) return null;

            return _list.Where(b => b.UserName == userName && b.Password == password).FirstOrDefault();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Exists(string userName)
        {
            if (_list == null || _list.Count < 1) ReadList();

            return _list.Exists(b => b.UserName == userName);
        }
        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="ID"></param>
        public static void Rem(string ID)
        {
            if (_list == null || _list.Count < 1) ReadList();

            var config = _list.Where(b => b.ID == ID).FirstOrDefault();

            if (config != null)
            {
                _list.Remove(config);
            }

            Save();
        }
    }
}

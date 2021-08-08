﻿using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;
using BaseApp.Data.DbModels;
using BaseApp.Data.Models;

namespace BaseApp.Data.Repositories
{
    public class UserRepository : IRepositoryBehavior
    {
        private readonly AuthenticationContext _context;
        public UserRepository(AuthenticationContext context)
        {
            _context = context;
        }

        private IEnumerable<AuthenticationUserAuditModel> All
        {
            get
            {
                return _context.Authentication.Join(_context.Users,
                    a => a.Id,
                    u => u.AuthenticationDbModelId,
                    (a, u) => new
                    {
                        Authentication = a,
                        User = u
                    }).Join(_context.Audit,
                    au => au.User.AuditDbModelId,
                    af => af.Id,
                    (au, af) => new
                    {
                        AuthenticationUser = au,
                        AuditFields = af
                    }).Select(aua => new AuthenticationUserAuditModel
                {
                    AuditDb = aua.AuditFields,
                    AuthenticationDb = aua.AuthenticationUser.Authentication,
                    UserDb = aua.AuthenticationUser.User
                });
            }
        }

        public IEnumerable<IDbModel> GetAll()
        {
            return All;
        }

        public IDbModel GetById(string id)
        {
            return All.FirstOrDefault(r => r.UserDb.Id == id);
        }

        public IEnumerable<IDbModel> GetByKeyValue(string key, string value)
        {
            return All.Where(r => value == (string)r.AuthenticationDb.GetType()
                .GetProperty(key)?
                .GetValue(r.AuthenticationDb));
        }

        public void Create(IDbModel dbModel)
        {
            var model = (UserDbModel)dbModel;
            if (model.AuthenticationDbModel == null || model.AuditDbModel == null)
            {
                CustomException
                    .Throw("No se puede guardar el usuario sin especificar sus parametros de autenticacion ni sus campos de Auditoria",
                        500);
            }
            _context.Users.Add(model);
            _context.SaveChanges();
        }

        public void Update(IDbModel dbModel)
        {
            var model = dbModel as AuthenticationUserAuditModel ?? new AuthenticationUserAuditModel();
            model.UserDb.AuthenticationDbModel = model.AuthenticationDb;
            model.UserDb.AuditDbModel = model.AuditDb;
            _context.SaveChanges();
        }

        public void Delete(IDbModel dbModel)
        {
            var model = dbModel as AuthenticationUserAuditModel ?? new AuthenticationUserAuditModel();
            _context.Audit.Remove(model.AuditDb);
            _context.Authentication.Remove(model.AuthenticationDb);
            _context.SaveChanges();
        }
    }
}
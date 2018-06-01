using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebHttpService
{
    public class Service : IService
    {
        private static IList<Student> employees = new List<Student>
        {
            new Student{ Id = "10000", Name="张三"},
            new Student{ Id = "10001", Name="李四"}
        };

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Student Get(string id)
        {
            var result = employees.FirstOrDefault(p => p.Id == id);
            if (null == result)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public List<Student> List()
        {
            return employees.ToList();
        }

        public string New(Student s)
        {
            throw new NotImplementedException();
        }

        public bool Update(Student s)
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class Student
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

    }
}

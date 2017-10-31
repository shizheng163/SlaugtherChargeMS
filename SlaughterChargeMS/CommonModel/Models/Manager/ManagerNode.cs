using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    public class ManagerNode
    {
        public int Id;
        public string LoginName;
        public string Name;
        public string OperatorName;
        public string OperatorTime;

        public ManagerNode()
        {

        }

        public ManagerNode(DB_Manager manager)
        {
            Id = manager.Id;
            LoginName = manager.LoginName;
            Name = manager.Name;
            OperatorName = manager.OperatorName;
            OperatorTime = manager.OperatorTime;
        }
    }
}

using iParking;
using iParking.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PGS_WEBAPI.Object;
using System;
using System.Collections.Generic;
using System.Data;

namespace PGS_WEBAPI.Controller
{
    [ApiController]
    [Route("/")]
    public class Controller : ControllerBase
    {
        [HttpGet("slots")]
        public ActionResult<List<Slot>> GetAllSlot()
        {
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            List<Slot> slots = new List<Slot>();
            string cmd = "Select Name,Status from tblZone order by Sort";
            DataTable dtSlots = StaticPool.mdb.FillData(cmd);
            if (dtSlots != null)
            {
                if (dtSlots.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSlots.Rows)
                    {
                        Slot slot = new Slot()
                        {
                            Name = row["Name"].ToString(),
                            Status = Convert.ToInt32(row["Status"].ToString())
                        };
                        slots.Add(slot);
                    }
                }
            }
            return Ok(slots);
        }

        [HttpGet("slots/{name}")]
        public ActionResult<Slot> GetSlotByID(string name)
        {
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            Slot slot = new Slot();
            string cmd = $"select Name,Status from tblZone where Name = '{name}'";
            DataTable dtSlotInfor = StaticPool.mdb.FillData(cmd);
            if (dtSlotInfor != null)
            {
                if (dtSlotInfor.Rows.Count > 0)
                {
                    DataRow row = dtSlotInfor.Rows[0];
                    slot.Name = row["Name"].ToString();
                    slot.Status = Convert.ToInt32(row["Status"].ToString());
                    return Ok(slot);
                }
            }
            slot.Name = name;
            slot.Status = 4;
            return Ok(slot);
        }

        [HttpGet("slotCount/{status}")]
        public ActionResult<GetSlotCountResponse> GetSlotCount(int status)
        {
            if (0 <= status && status < 4)
            {
                if (StaticPool.mdb == null)
                {
                    StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
                }
                int totalSlot = 0;
                string cmdGetTotalCount = $"SELECT COUNT(ID) as Count FROM tblZone";
                DataTable dtTotalSlots = StaticPool.mdb.FillData(cmdGetTotalCount);
                if (dtTotalSlots != null)
                {
                    totalSlot = Convert.ToInt32(dtTotalSlots.Rows[0]["Count"].ToString());
                }
                if (totalSlot > 0)
                {
                    string cmd = $"SELECT COUNT(ID) as Count FROM tblZone WHERE Status = {status}";
                    DataTable dtSlots = StaticPool.mdb.FillData(cmd);
                    if (dtSlots != null)
                    {
                        if (dtSlots.Rows.Count > 0)
                        {
                            GetSlotCountResponse slotCountResponse = new GetSlotCountResponse()
                            {
                                SlotStatus = status,
                                SlotCount = Convert.ToInt32(dtSlots.Rows[0]["Count"].ToString()),
                                TotalSlot = totalSlot,
                            };
                            return Ok(slotCountResponse);
                        }
                    }
                }
                GetSlotCountResponse slotCountResponse_empty = new GetSlotCountResponse()
                {
                    SlotStatus = status,
                    SlotCount = 0,
                    TotalSlot = totalSlot,
                };
                return Ok(slotCountResponse_empty);
            }
            else
            {
                return BadRequest("Status must be 0, 1, 2 or 3");
            }
        }

        [HttpPost("slots/LastStatusEvent")]
        public ActionResult<GetSlotCountResponse> GetLastStatusEvent(LastStatus lastStatus)
        {
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            DataTable dtLastStatus = StaticPool.mdb.FillData($"select top 1 Date from tblZoneEvent where  OldStatus = {lastStatus.OldStatus} And Status = {lastStatus.Status} order by Date Desc");
            if (dtLastStatus != null)
            {
                lastStatus.LastEventTime = DateTime.MinValue;
            }
            if (dtLastStatus.Rows.Count > 0)
            {
                lastStatus.LastEventTime = DateTime.Parse(dtLastStatus.Rows[0]["Date"].ToString());
            }
            return Ok(lastStatus);
        }

        [HttpGet("pgs/detail")]
        public ActionResult<GetSlotCountResponse> GetPGSDetail()
        {
            Dictionary<int, int> pgs_details = new Dictionary<int, int>();
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            PGS_Detail pgs_Detail = new PGS_Detail();
            pgs_Detail.TotalSlot = 0;
            pgs_Detail.Detail.Add(0, 0);
            pgs_Detail.Detail.Add(1, 0);
            pgs_Detail.Detail.Add(2, 0);
            int totalSlot = 0;
            string cmdGetTotalCount = $"SELECT COUNT(ID) as Count FROM tblZone";
            DataTable dtTotalSlots = StaticPool.mdb.FillData(cmdGetTotalCount);
            if (dtTotalSlots != null)
            {
                totalSlot = Convert.ToInt32(dtTotalSlots.Rows[0]["Count"].ToString());
            }
            if (totalSlot > 0)
            {
                pgs_Detail.TotalSlot = totalSlot;
                string cmd = $"SELECT Status,COUNT(ID) as Count FROM tblZone group by Status";
                DataTable dtSlots = StaticPool.mdb.FillData(cmd);
                if (dtSlots != null)
                {
                    if (dtSlots.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtSlots.Rows)
                        {
                            int status = Convert.ToInt32(row["Status"].ToString());
                            if (status >= 0 || status <= 2)
                            {
                                pgs_Detail.Detail[status] = Convert.ToInt32(row["Count"].ToString());
                            }
                        }
                    }
                }
            }
            return Ok(pgs_Detail);
        }

        [HttpGet("groups/{name}")]
        public ActionResult<GroupDetail> GetGroupByName(string name)
        {
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            GroupDetail group = new GroupDetail();
            string cmd = $@"select sum(case when groupDetail.Status = 0 then 1 else 0 end) as UnOccupied ,sum(case when groupDetail.Status = 1 then 1 else 0 end) as Occupied  
                                from (select tblZone.Status as Status
                                from tblZone,tblZoneGroup 
                                where tblZone.GroupID=tblZoneGroup.id AND tblZoneGroup.Name = '{name}') as groupDetail";
            DataTable dtGroupSlotDetail = StaticPool.mdb.FillData(cmd);
            if (dtGroupSlotDetail != null)
            {
                if (dtGroupSlotDetail.Rows.Count > 0)
                {
                    DataRow row = dtGroupSlotDetail.Rows[0];
                    group.UnOccupiedCount = Convert.ToInt32(row["UnOccupied"].ToString());
                    group.OccupiedCount = Convert.ToInt32(row["Occupied"].ToString());
                    DataTable dtGroup = StaticPool.mdb.FillData($@"select tblZoneGroup.ID,tblZoneGroup.Name,tblZoneGroup.Code 
                                                                       from tblZoneGroup 
                                                                       where tblZoneGroup.Name = '{name}' ");
                    if (dtGroup != null)
                    {
                        if (dtGroup.Rows.Count > 0)
                        {
                            group.GroupName = dtGroup.Rows[0]["Name"].ToString();
                            group.GroupCode = dtGroup.Rows[0]["Code"].ToString();
                            return Ok(group);
                        }
                    }
                }
            }
            return Ok("Name Invalid");
        }

        [HttpPost("Parking/CompareParkingNumber/{parkingNumber}")]
        public ActionResult<bool> CompareParkingNumber(int parkingNumber)
        {
            if (StaticPool.mdb == null)
            {
                StaticPool.mdb = new MDB(StaticPool.SQLServerName, StaticPool.SQLDatabaseName, StaticPool.SQLAuthentication, StaticPool.SQLUserName, StaticPool.SQLPassword);
            }
            //Get Last Compare Event
            int lastEvent_PGSNumber = 0;
            int lastEvent_ParkingNumber = 0;
            int lastEvent_PGS_Total_Number = 0;
            DataTable dtLastCompareEvent = StaticPool.mdb.FillData("SELECT TOP (1) [ParkingNumber],[PGSNumber],[PGS_Total_Number] from [tblParkingComparisons] order by CreatedDate desc");
            if (dtLastCompareEvent != null)
            {
                if (dtLastCompareEvent.Rows.Count > 0)
                {
                    lastEvent_PGSNumber = Convert.ToInt32(dtLastCompareEvent.Rows[0]["PGSNumber"].ToString());
                    lastEvent_ParkingNumber = Convert.ToInt32(dtLastCompareEvent.Rows[0]["ParkingNumber"].ToString());
                    lastEvent_PGS_Total_Number = Convert.ToInt32(dtLastCompareEvent.Rows[0]["PGS_Total_Number"].ToString());
                }
            }
            //Get Current PGS Count
            int currentOccupiedPgsNumber = 0;
            string cmd = $"SELECT COUNT(ID) as Count FROM tblZone WHERE Status = 1";
            DataTable dtSlots = StaticPool.mdb.FillData(cmd);
            if (dtSlots != null)
            {
                if (dtSlots.Rows.Count > 0)
                {
                    currentOccupiedPgsNumber = Convert.ToInt32(dtSlots.Rows[0]["Count"].ToString());
                }
            }
            int totalSlot = 0;
            string cmdGetTotalCount = $"SELECT COUNT(ID) as Count FROM tblZone";
            DataTable dtTotalSlots = StaticPool.mdb.FillData(cmdGetTotalCount);
            if (dtTotalSlots != null)
            {
                totalSlot = Convert.ToInt32(dtTotalSlots.Rows[0]["Count"].ToString());
            }
            ParkingComparison parkingComparisons = new ParkingComparison()
            {
                ParkingNumber = parkingNumber
            };
            //Check is new Event?
            if (lastEvent_ParkingNumber == parkingComparisons.ParkingNumber && lastEvent_PGSNumber == currentOccupiedPgsNumber)
            {
                if (totalSlot == lastEvent_PGS_Total_Number)
                    return Ok(true);
            }
            {
                //Get Current parking count
                int currentParkingCount = parkingComparisons.ParkingNumber;

                parkingComparisons.PGSNumber = currentOccupiedPgsNumber;
                parkingComparisons.CreatedDate = DateTime.Now;
                parkingComparisons.Different = parkingComparisons.PGSNumber - parkingComparisons.ParkingNumber;
                if (parkingComparisons.Different == 0)
                {
                    if (parkingComparisons.PGSNumber == totalSlot)
                    {
                        parkingComparisons.EventCode = 4;
                    }
                    else
                    {
                        parkingComparisons.EventCode = 0;
                    }
                }
                else if (parkingComparisons.Different > 0)
                {
                    parkingComparisons.EventCode = 1;
                }
                else if (parkingComparisons.Different < 0)
                {
                    if (parkingComparisons.ParkingNumber <= totalSlot)
                        parkingComparisons.EventCode = 2;
                    else
                        parkingComparisons.EventCode = 3;
                }
                //Insert New Event
                string insertCMD = $@"insert into tblParkingComparisons(EventCode,CreatedDate,ParkingNumber,PGSNumber,Different,PGS_Total_Number) 
                                                                      Values({parkingComparisons.EventCode},'{DateTime.Now}',{parkingComparisons.ParkingNumber},{parkingComparisons.PGSNumber},{Math.Abs(parkingComparisons.Different)},{totalSlot})";
                bool result = StaticPool.mdb.ExecuteCommand(insertCMD);
                return Ok(result);
            }
        }

        [HttpGet("Parking/ReportCompareParkingNumber")]
        public ActionResult<List<ParkingComparison>> GetReportCompareParkingNumber()
        {
            List<ParkingComparison> parkingComparisons = new List<ParkingComparison>();
            StringValues fromDate = default(StringValues);
            StringValues ToDate = default(StringValues);
            StringValues eventCode = default(StringValues);
            if (this.Request.Headers.ContainsKey("FromDate"))
            {
                this.Request.Headers.TryGetValue("FromDate", out fromDate);
            }
            if (this.Request.Headers.ContainsKey("ToDate"))
            {
                this.Request.Headers.TryGetValue("ToDate", out ToDate);
            }
            if (this.Request.Headers.ContainsKey("EventCode"))
            {
                this.Request.Headers.TryGetValue("EventCode", out eventCode);
            }
            DateTime _fromDate = DateTime.Parse(fromDate.ToString());
            DateTime _toDate = DateTime.Parse(ToDate.ToString());
            int _eventCode = Convert.ToInt32(eventCode.ToString());
            string GetReportCMD = "";
            if (_eventCode == -1)
            {
                GetReportCMD = $@"Select * from [tblParkingComparisons] where (cast(tblParkingComparisons.CreatedDate as datetime) between  '{_fromDate}' AND '{_toDate}')";
            }
            else
            {
                GetReportCMD = $@"Select * from [tblParkingComparisons] where EventCode = {eventCode} AND  (cast(tblParkingComparisons.CreatedDate as datetime) between  '{_fromDate}' AND '{_toDate}')";
            }
            DataTable dtReport = StaticPool.mdb.FillData(GetReportCMD);
            if (dtReport != null)
            {
                if (dtReport.Rows.Count > 0)
                {
                    foreach (DataRow row in dtReport.Rows)
                    {
                        ParkingComparison parkingComparison = new ParkingComparison()
                        {
                            CreatedDate = DateTime.Parse(row["CreatedDate"].ToString()),
                            EventCode = Convert.ToInt32(row["EventCode"].ToString()),
                            ParkingNumber = Convert.ToInt32(row["ParkingNumber"].ToString()),
                            PGSNumber = Convert.ToInt32(row["PGSNumber"].ToString()),
                            Different = Convert.ToInt32(row["Different"].ToString()),
                            PGS_Total_Number = Convert.ToInt32(row["PGS_Total_Number"].ToString()),
                        };
                        parkingComparisons.Add(parkingComparison);
                    }
                }
            }
            return Ok(parkingComparisons);
        }

        [HttpGet("GetParkingStateByDatetime/{searchTime}")]
        public ActionResult<List<ParkingComparison>> GetReportCompareParkingNumber(string searchTime)
        {
            DateTime _fromDate = DateTime.Parse(searchTime);
            string GetReportCMD = "";
            string searchCMD = $@"SELECT TOP (1) [EventCode]
      ,[CreatedDate]
      ,[ParkingNumber]
      ,[PGSNumber]
      ,[Different]
      ,[ZoneImage1]
      ,[ZoneImage2]
      ,[PGS_Total_Number]
  FROM [tblParkingComparisons] where (cast(tblParkingComparisons.CreatedDate as datetime) <=  '{_fromDate}') order by CreatedDate desc";
            GetReportCMD = searchCMD;
            DataTable dtReport = StaticPool.mdb.FillData(GetReportCMD);
            if (dtReport != null)
            {
                if (dtReport.Rows.Count > 0)
                {
                    DataRow lastestRow = dtReport.Rows[0];
                    ParkingComparison parkingComparison = new ParkingComparison()
                    {
                        CreatedDate = DateTime.Parse(lastestRow["CreatedDate"].ToString()),
                        EventCode = Convert.ToInt32(lastestRow["EventCode"].ToString()),
                        ParkingNumber = Convert.ToInt32(lastestRow["ParkingNumber"].ToString()),
                        PGSNumber = Convert.ToInt32(lastestRow["PGSNumber"].ToString()),
                        Different = Convert.ToInt32(lastestRow["Different"].ToString()),
                        PGS_Total_Number = Convert.ToInt32(lastestRow["PGS_Total_Number"].ToString()),
                    };
                    return Ok(parkingComparison);
                }
            }
            return Ok(null);
        }
        public class ParkingComparison
        {
            public int EventCode { get; set; }
            public DateTime CreatedDate { get; set; }
            public int ParkingNumber { get; set; }
            public int PGSNumber { get; set; }
            public string ZoneImage1 { get; set; }
            public string ZoneImage2 { get; set; }
            public int Different { get; set; }
            public int PGS_Total_Number { get; set; }
        }

        public class PGS_Detail
        {
            public int TotalSlot { get; set; }
            public Dictionary<int, int> Detail { get; set; } = new Dictionary<int, int>();
        }

        public class GetSlotCountResponse
        {
            public int SlotStatus { get; set; }
            public int SlotCount { get; set; }
            public int TotalSlot { get; set; }
        }

        public class LastStatus
        {
            public int OldStatus { get; set; }
            public int Status { get; set; }
            public DateTime LastEventTime { get; set; }
        }
    }
}
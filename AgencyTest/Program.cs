using DB;
using OraDB;
using System;

namespace AgencyTest
{
    class Program
    {
        static void Main(string[] args) { }

        //approval/reject profiletypeid

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add OwnershipType Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyAddOwnershipType(name);
        //}
        //ADD OWNERSHIP TYPE


        //{
        //    Console.WriteLine("Add Business Type Name");
        //    DBHandler dBHandler = new DBHandler();

        //    var res = dBHandler.AgencyAddBusinessType("Agency");

        //    var agencyAddBusinessType = Console.ReadLine();

        //    Console.WriteLine(agencyAddBusinessType);

        //    dBHandler.AgencyAddBusinessType(agencyAddBusinessType);

        //}
        //ADD BUSINESS TYPE


        //{
        //    Console.WriteLine("Add Profile Type Name");
        //    DBHandler dBHandler = new DBHandler();

        //    var res = dBHandler.AgencyAddProfileType("Agency");

        //    var agencyAddProfileType = Console.ReadLine();

        //    Console.WriteLine(agencyAddProfileType);

        //    dBHandler.AgencyAddProfileType(agencyAddProfileType);

        //}
        //ADD PROFILE TYPE


        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Reason Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyAddReason(name);
        //}
        //Add Reasons

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key in Reason ID to be deleted.");
        //    var reasonid = Console.ReadLine();
        //    dBHandler.AgencyDeleteReason(Int32.Parse(reasonid));
        //}
        //Delete Reasons


        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Reason Type Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyAddReasonType(name);
        //}
        //Add Reason Type


        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Profile Name");
        //    var name = Console.ReadLine();
        //    Console.WriteLine("Add profiletype id");
        //    var profiletype_id = Console.ReadLine();

        //    dBHandler.AgencyAddProfile(name, Int32.Parse(profiletype_id));
        //}
        //ADD PROFILE

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add BankName");
        //    var bankName = Console.ReadLine();
        //    Console.WriteLine("Swift code");
        //    var swiftCode = Console.ReadLine();
        //    Console.WriteLine("Bank code");
        //    var bankCode = Console.ReadLine();
        //    dBHandler.AgencyAddBank(bankName, Int32.Parse(swiftCode), bankCode);
        //}
        //}
        //}//Add bank

        //    {
        //        DBHandler dBHandler = new DBHandler();
        //        Console.WriteLine("Key In Bank ID To Delete");
        //        var bankid = Console.ReadLine();
        //        dBHandler.AgencyDeleteBank(Int32.Parse(bankid));
        //    }//delete bank
        //}

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add BankName");
        //    var bankName = Console.ReadLine();
        //    Console.WriteLine("Add Bank Branch Code");
        //    var branchCode = Console.ReadLine();
        //    Console.WriteLine("Bank ID");
        //    var bankid = Console.ReadLine();
        //    dBHandler.AgencyAddBankBranch(bankName, branchCode, Int32.Parse(bankid));
        //}bankbranch

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Device Serialnumber ");
        //    var serialnumber = Console.ReadLine();
        //    dBHandler.AgencyAddDevice(serialnumber);
        //} //add device

        //{
        //        DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add CustomerPhoneNumber");
        //    var customerphonenumber = Console.ReadLine();
        //    Console.WriteLine("Complaint Description");
        //    var Description = Console.ReadLine();
        //    Console.WriteLine("Was it Resolved? (0/1)");
        //    var resolved = Console.ReadLine();
        //    Console.WriteLine("Remarks");
        //    var Remarks = Console.ReadLine();
        //    dBHandler.AgencyAddComplaint(Int32.Parse(customerphonenumber), Int32.Parse(resolved), Remarks, Description);
        //}Complaint

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Ownershiptype ID");
        //    var ownershiptypeid = Console.ReadLine();
        //    Console.WriteLine("New Ownershipetype Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyUpdateOwnershiptype(Int32.Parse(ownershiptypeid), name);
        //}
        //updateownershiptype

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert BusinessType ID");
        //    var businesstypeid = Console.ReadLine();
        //    Console.WriteLine("New BusinessType Name");
        //    var businesstype = Console.ReadLine();
        //    dBHandler.AgencyUpdateBusinessType(Int32.Parse(businesstypeid), businesstype);
        //}
        //updatebusinesstype

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert BusinessType ID to be deleted.");
        //    var businesstypeid = Console.ReadLine();
        //    dBHandler.AgencyDeleteBusinessType(Int32.Parse(businesstypeid));
        //} //delete businesstype

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Profiletype ID");
        //    var profiletypeid = Console.ReadLine();
        //    Console.WriteLine("New Profiletype Name");
        //    var profiletype = Console.ReadLine();
        //    dBHandler.AgencyUpdateProfileType(Int32.Parse(profiletypeid), profiletype);
        //}updateprofiletype

        //{
        //    DBHandler dBHandler = new DBHandler();

        //    Console.WriteLine("Insert Profile ID");
        //    var profileid = Console.ReadLine();
        //    Console.WriteLine("New Profile Name");
        //    var profile = Console.ReadLine();
        //    dBHandler.AgencyUpdateProfile(Int32.Parse(profileid), profile);
        //}updateprofile

        //{
        //    DBHandler dBHandler = new DBHandler();

        //    Console.WriteLine("Insert Reason ID");
        //    var reasonid = Console.ReadLine();
        //    Console.WriteLine("New Reason Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyUpdateReason(Int32.Parse(reasonid), name);
        //}
        // update reason

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Reason Type ID");
        //    var id = Console.ReadLine();
        //    Console.WriteLine("New Reason Type Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyUpdateReasonType(Int32.Parse(id), name);
        //}
        //Update Reason Type

        //{
        //    DBHandler dBHandler = new DBHandler();

        //    Console.WriteLine("Insert Device ID");
        //    var deviceid = Console.ReadLine();
        //    Console.WriteLine("New Device Serialnumber");
        //    var serialnumber = Console.ReadLine();
        //    dBHandler.AgencyUpdateDevice(Int32.Parse(deviceid), serialnumber);
        //}
        //update device

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Customer Phone Number");
        //    var customerphonenumber = Console.ReadLine();
        //    Console.WriteLine("Was it resolved?(0,1)");
        //    var resolved = Console.ReadLine();
        //    Console.WriteLine("New Remarks (If none, proceed>>)");
        //    var remarks = Console.ReadLine();
        //    Console.WriteLine("New Complaint Description(If none, proceed>>)");
        //    var complaintdesc = Console.ReadLine();
        //    dBHandler.AgencyUpdateComplaint(Int32.Parse(customerphonenumber),Int32.Parse( resolved), remarks, complaintdesc);
        //}Updatecomplaint

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Bank ID");
        //    var bankid = Console.ReadLine();
        //    Console.WriteLine("Insert Bank Name (If new, type the new name as well>>)");
        //    var bankname = Console.ReadLine();
        //    Console.WriteLine("New Bank Code (If none, proceed>>)");
        //    var bankcode = Console.ReadLine();
        //    Console.WriteLine("New Swift Code(If none, proceed>>)");
        //    var swiftcode = Console.ReadLine();
        //    dBHandler.AgencyUpdateBank(Int32.Parse(bankid), bankname, bankcode, swiftcode);
        //}UpdateBank

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The BankID");
        //    var bankid = Console.ReadLine();
        //    Console.WriteLine("Insert Bank Branch Id");
        //    var branchid = Console.ReadLine();
        //    Console.WriteLine("Insert Bank Name (If new, type the new name as well>>)");
        //    var bankname = Console.ReadLine();
        //    Console.WriteLine("New Branch Code (If none, proceed>>)");
        //    var branchcode = Console.ReadLine();
        //    dBHandler.AgencyUpdateBankBranch(Int32.Parse(branchid), bankname, branchcode, Int32.Parse(bankid));
        //}
        //BankBranch

        //{
        //     DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The Agent Holder Id");
        //    var agentholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddAgentApproval(Int32.Parse(agentholderid), Boolean.Parse(approve));
        //}
        //agent_approval

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Firstname");
        //    var firstname = Console.ReadLine();
        //    Console.WriteLine("Insert Lastname");
        //    var lastname = Console.ReadLine();
        //    Console.WriteLine("Insert Surname");
        //    var surname = Console.ReadLine();
        //    Console.WriteLine("Insert PhoneNumber");
        //    var phone = Console.ReadLine();
        //    Console.WriteLine("Insert Emailaddress");
        //    var emailaddress = Console.ReadLine();
        //    Console.WriteLine("Insert Username");
        //    var username = Console.ReadLine();
        //    Console.WriteLine("Insert The password");
        //    var password = Console.ReadLine();
        //    Console.WriteLine("Insert OTP");
        //    var otp = Console.ReadLine();
        //    Console.WriteLine("Insert Profileid");
        //    var profileid = Console.ReadLine();
        //    Console.WriteLine("Insert Agentid");
        //    var agentid = Console.ReadLine();
        //    Console.WriteLine("Insert Agentoutletid");
        //    var agentoutletid = Console.ReadLine();
        //    dBHandler.AgencyAddAgentStaff(firstname, lastname, surname, phone, emailaddress, username, password, otp, Int32.Parse(profileid), Int32.Parse(agentid), Int32.Parse(agentoutletid));
        //}
        //Add agent staff
        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The Agent Staff Holder Id");
        //    var agentstaffid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddAgentStaffApproval(Int32.Parse(agentstaffid), Boolean.Parse(approve));
        //}//approve/reject agent staff

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Agent Staff ID To Delete");
        //    var agentstaffid = Console.ReadLine();
        //    dBHandler.AgencyDeleteAgentStaff(Int32.Parse(agentstaffid));
        //}//Delete agent staff


        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert Agent Staff ID");
        //    var agentstaffid = Console.ReadLine();
        //    Console.WriteLine("New First Name (If none proceed)");
        //    var firstname = Console.ReadLine();
        //    Console.WriteLine("New Last Name (If none proceed)");
        //    var lastname = Console.ReadLine();
        //    Console.WriteLine("New Surname (If none proceed)");
        //    var surname = Console.ReadLine();
        //    Console.WriteLine("New Phone Number (If none, proceed>>)");
        //    var phone = Console.ReadLine();
        //    Console.WriteLine("New Email Address (If none proceed)");
        //    var emailaddress = Console.ReadLine();
        //    Console.WriteLine("New Username (If none proceed)");
        //    var username = Console.ReadLine();
        //    Console.WriteLine("New password (If none, proceed>>)");
        //    var password = Console.ReadLine();
        //    //Console.WriteLine("New OTP");
        //    //var otp = Console.ReadLine();
        //    //Console.WriteLine("New OTP Date");
        //    //var otpdate = Console.ReadLine();
        //    //Console.WriteLine("Date of Update");
        //    //var datemodified = Console.ReadLine();
        //    Console.WriteLine("New Profile ID (If none, proceed>>)");
        //    var profileid = Console.ReadLine();
        //    Console.WriteLine("New Agent ID (If none, proceed>>)");
        //    var agentid = Console.ReadLine();
        //    Console.WriteLine("New Agent Outlet ID (If none, proceed>>)");
        //   var agentoutletid = Console.ReadLine();

        //    dBHandler.AgencyUpdateAgentStaff(Int32.Parse(agentstaffid), firstname, lastname, surname, phone, emailaddress, username, password,/* otp, otpdate, datemodified,*/  Int32.Parse(profileid), Int32.Parse(agentid), Int32.Parse(agentoutletid));
        //}//updateagentstaff
        // //
        // //--TO BE AMMENDED--

        //{
        //    //DBHandler dBHandler = new DBHandler();
        //    //Console.WriteLine("Insert The Agent Staff Id");
        //    //var agentstaffid = Console.ReadLine();
        //    //Console.WriteLine("False To Reject or True To Approve");
        //    //var approve = Console.ReadLine();
        //    //dBHandler.AgencyAddAgentStaffApproval(Int32.Parse(agentstaffid), Boolean.Parse(approve));
        //} //approve_reject_agentstaff

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The Device Id");
        //    var deviceholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddDeviceApproval(Int32.Parse(deviceholderid), Boolean.Parse(approve));
        //}//approved/rejectaddeddevice

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key in Superagentid");
        //    var superagentid = Console.ReadLine();
        //    Console.WriteLine("Key in Businessname");
        //    var businessname = Console.ReadLine();
        //    Console.WriteLine("Key in Emailaddress");
        //    var emailaddress = Console.ReadLine();
        //    Console.WriteLine("Key in Phone");
        //    var phone = Console.ReadLine();
        //    Console.WriteLine("Key in Address");
        //    var address = Console.ReadLine();
        //    Console.WriteLine("Key in Bizregcert");
        //    var bizregcert = Console.ReadLine();
        //    Console.WriteLine("Key in Bizlicense");
        //    var bizlicense = Console.ReadLine();
        //    Console.WriteLine("Key in Financialstatement");
        //    var financialstatement = Console.ReadLine();
        //    Console.WriteLine("Key in Goodconductcert");
        //    var goodconductcert = Console.ReadLine();
        //    Console.WriteLine("Key in Businesstypeid");
        //    var businesstypeid = Console.ReadLine();
        //    Console.WriteLine("Key in Ownershiptypeid");
        //    var ownershiptypeid = Console.ReadLine();
        //    dBHandler.AgencyAddAgent(Int32.Parse(superagentid),businessname,emailaddress,phone,address,bizregcert,bizlicense,financialstatement,goodconductcert,Int32.Parse(businesstypeid),Int32.Parse(ownershiptypeid));

        //}//add agent

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The Agent Staff Id");
        //    var agentid = Console.ReadLine();
        //    dBHandler.AgencyDeleteAgent(Int32.Parse(agentid));
        //} //delete agent

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Update Superagentid, if no proceed.>>");
        //    var superagentid = Console.ReadLine();
        //    Console.WriteLine("Update Businessname , if no proceed.>>");
        //    var businessname = Console.ReadLine();
        //    Console.WriteLine("Update Emailaddress , if no proceed.>>");
        //    var emailaddress = Console.ReadLine();
        //    Console.WriteLine("Update Phone , if no proceed.>>");
        //    var phone = Console.ReadLine();
        //    Console.WriteLine("Update Address , if no proceed.>>");
        //    var address = Console.ReadLine();
        //    Console.WriteLine("Update  Bizregcert  , if no proceed.>>");
        //    var bizregcert = Console.ReadLine();
        //    Console.WriteLine("Update  Bizlicense  , if no proceed.>>");
        //    var bizlicense = Console.ReadLine();
        //    Console.WriteLine("Update  Financialstatement , if no proceed.>>");
        //    var financialstatement = Console.ReadLine();
        //    Console.WriteLine("Update  Goodconductcert , if no proceed.>>");
        //    var goodconductcert = Console.ReadLine();
        //    Console.WriteLine("Update  Businesstypeid , if no proceed.>>");
        //    var businesstypeid = Console.ReadLine();
        //    Console.WriteLine("Update  Ownershiptypeid , if no proceed.>>");
        //    var ownershiptypeid = Console.ReadLine();
        //    dBHandler.AgencyUpdateAgent(Int32.Parse(superagentid), businessname, emailaddress, phone, address, bizregcert, bizlicense, financialstatement, goodconductcert, Int32.Parse(businesstypeid), Int32.Parse(ownershiptypeid));
        //} //update agent

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Agent Service");
        //    var name = Console.ReadLine();
        //    Console.WriteLine("Add Service Code");
        //    var servicecode = Console.ReadLine();
        //    dBHandler.AgencyAddAgentService(name, servicecode);
        //} //add agent service

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Agent Service ID");
        //    var agentserviceholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddAgentServiceApproval(Int32.Parse(agentserviceholderid), Boolean.Parse( approve));
        //}//approve/reject agentservice

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Agent Service ID To Delete");
        //    var agentserviceid = Console.ReadLine();
        //    dBHandler.AgencyDeleteAgentService(Int32.Parse(agentserviceid));
        //}//delete agent service
        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Agent Service ID");
        //    var agentserviceid = Console.ReadLine();
        //    Console.WriteLine("Key In New Agent Service Name, if none proceed>>");
        //    var name = Console.ReadLine();
        //    Console.WriteLine("Key In New Agent Service Code, if none proceed>>");
        //    var servicecode = Console.ReadLine();
        //    dBHandler.AgencyUpdateAgentService(Int32.Parse(agentserviceid),name,servicecode);
        //}//update agent service   to be ammended


        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Bank Branch ID To Delete");
        //    var bankbranchid = Console.ReadLine();
        //    dBHandler.AgencyDeleteBankBranch(Int32.Parse(bankbranchid));
        //}//delete bank branch

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Bank Branch Name");
        //    var bankbranchname = Console.ReadLine();
        //    Console.WriteLine("Add Bank Branch Code");
        //    var bankbranchcode = Console.ReadLine();
        //    Console.WriteLine("Add Bank ID");
        //    var bankid = Console.ReadLine();
        //    dBHandler.AgencyAddBankBranch(bankbranchname, bankbranchcode, Int32.Parse(bankid));
        //} //Addbankbranch

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add firstname");
        //    var firstname = Console.ReadLine();
        //    Console.WriteLine("Add lastname");
        //    var lastname = Console.ReadLine();
        //    Console.WriteLine("Add surname");
        //    var surname = Console.ReadLine();
        //    Console.WriteLine("Add phone number");
        //    var phone = Console.ReadLine();
        //    Console.WriteLine("Add emailaddress");
        //    var emailaddress = Console.ReadLine();
        //    Console.WriteLine("Add username");
        //    var username = Console.ReadLine();
        //    Console.WriteLine("Add password");
        //    var password = Console.ReadLine();
        //    Console.WriteLine("Add profileid");
        //    var profileid = Console.ReadLine();
        //    Console.WriteLine("Add bankid");
        //    var bankid = Console.ReadLine();
        //    dBHandler.AgencyAddBankStaff(firstname, lastname, surname, phone, emailaddress, username, password, Int32.Parse(profileid) , Int32.Parse(bankid));
        /* }*/ //add bankstaff

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Bank Staff ID");
        //    var bankstaffholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddBankStaffApproval(Int32.Parse(bankstaffholderid), Boolean.Parse(approve));
        //} //approve/reject bank staff approval

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Bank Staff ID To Delete");
        //    var bankstaffid = Console.ReadLine();
        //    dBHandler.AgencyDeleteBankStaff(Int32.Parse(bankstaffid));
        //} //delete bank staff

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Insert The Device Id");
        //    var deviceid = Console.ReadLine();
        //    dBHandler.AgencyDeleteDevice(Int32.Parse(deviceid));
        //} //delete device

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Ownershiptype ID To Delete");
        //    var ownershiptypeid = Console.ReadLine();
        //    dBHandler.AgencyDeleteOwnershiptype(Int32.Parse(ownershiptypeid));
        //} //delete ownershiptype

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Profile ID");
        //    var profileholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddProfileApproval(Int32.Parse(profileholderid), Boolean.Parse(approve));
        //} approve/reject profile

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Profile ID To Delete");
        //    var profileid = Console.ReadLine();
        //    dBHandler.AgencyDeleteProfile(Int32.Parse(profileid));
        //}
        //delete profile

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add ProfileType Name");
        //    var name = Console.ReadLine();
        //    dBHandler.AgencyAddProfileType(name);
        //} //add profiletype

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Profile Type ID");
        //    var profiletypeholderid = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var approve = Console.ReadLine();
        //    dBHandler.AgencyAddProfileTypeApproval(Int32.Parse(profiletypeholderid), Boolean.Parse(approve));
        //} //approval/reject profiletypeid

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Profile Type ID To Delete");
        //    var profiletypeid = Console.ReadLine();
        //    dBHandler.AgencyDeleteProfileType(Int32.Parse(profiletypeid));
        //} /*deleteprofiletype*/

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Reason Type ID To Delete");
        //    var reasontypeid = Console.ReadLine();
        //    dBHandler.AgencyDeleteReasontype(Int32.Parse(reasontypeid));
        //} //delete reason type

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Add Item Key");
        //    var item_key = Console.ReadLine();
        //    Console.WriteLine("Add Item Name");
        //    var item_name = Console.ReadLine();
        //    Console.WriteLine("False To Reject or True To Approve");
        //    var is_encrypted = Console.ReadLine();
        //    dBHandler.AgencyAddParameter(item_key, item_name,Boolean.Parse(is_encrypted));

        //} //add parameter

        //{
        //    DBHandler dBHandler = new DBHandler();
        //    Console.WriteLine("Key In Parameter ID To Delete");
        //    var parameterid = Console.ReadLine();
        //    dBHandler.AgencyDeleteParameter(Int32.Parse(parameterid));
        //} //delete reason type
    }
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Utils
{
    public class MessageConstant
    {
        // message of portfolio
        public static string DUPLICATE_NAME = "Name is already exist! Please use other name!";
        public static string NOT_FOUND_PORTFOLIO = "Portfolio not found!";
        public static string NOT_FOUND_SKILL = "Skill not found!";
        public static string NOT_FOUND_EDUCATION = "Education not found!";
        public static string NOT_FOUND_EXPERIENCE = "Experience not found!";
        public static string NOT_FOUND_PRODUCT = "Product not found!";
        public static string NOT_PUBLIC_PORTFOLIO = "Portfolio not public!";
        public static string NULL_PROFILE_PORTFOLIO = "Please create profile before go to Portfolio page!";
        public static string MAIN_EXISTED = "Main Status Existed";
        public static string NULL_MAIN_PORTFOLIO = "Please go to User Profile and set your main portfolio before apply this recruitment!";

        // common message 
        public static string DATA_NOT_EMPTY = "Data field not empty";
        public static string EMAIL_EXIST = "Email is exist";
        public static string UPDATE_SUCCESS = "Update success";
        public static string CREATE_SUCCESS = "Create success";
        public static string DELETE_SUCCESS = "Delete success";
        public static string UPDATE_FAIL = "Update fail";
        public static string CREATE_FAIL = "Create fail";
        public static string DELETE_FAIL = "Delete fail";
        public static string APPLY_SUCCESS = "Aplly successful!";
        public static string APPLY_FAIL = "Aplly fail!";
        public static string APPLY_ALREADY = "You already apply to this recruitment!";
        public static string ADD_FAILED = "Add Failed! ";
        public static string ADD_SUCCESS = "Add Successfully!";  

        // admin message 
        public static string APPROVE_SUCCESS = "Approved Successfully";
        public static string APPROVE_FAIL = "Approved Failed";
        public static string REJECT_SUCCESS = "Reject Successfully";
        public static string REJECT_FAILED = "Approved Failed";        
        public static string ADD_FAILED_DATA_EMPTY = ADD_FAILED + DATA_NOT_EMPTY;       
        public static string UPDATE_FAILED_DATA_EMPTY = UPDATE_FAIL + DATA_NOT_EMPTY;

        // partner messate
        public static string NOT_FOUND_RECRUITMENT = "Recruitment not found!";
        public static string NOT_FOUND_COMPANY = "Company not found!";
        public static string WRONG_INFO_CANDIDATE = "Candidate information was wrong";
        public static string NOT_APPROVE_YET_COMPANY = "The status of the company is not approved yet";
    }
}
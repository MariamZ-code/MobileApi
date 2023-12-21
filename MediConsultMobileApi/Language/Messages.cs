namespace MediConsultMobileApi.Language
{
    public class Messages
    {
        // Member  
        public static string MemberNotFound(string language)
        {
            return language == "ar" ? "المستخدم غير موجود" : "Member not found";
        }
      
        public static string MemberArchive(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Archive";
        }
        public static string MemberDeactivated(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Deactivated";
        }
        public static string MemberHold(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Hold";
        }
        public static string MemberChange(string language)
        {
            return language == "ar" ? "تم التعديل بنجاح" : "Modified successfully";
        }

        public static string MemberLoginExists(string language)
        {
            return language == "ar" ? " المستخدم ليس لديه حساب" : "Member  not have Account";
        }

        public static string EnterMember(string language)
        {
            return language == "ar" ? "ادخل رقم المستخدم" : "Enter member Id";
        }

        // Email Validation

        public static string Emailexist(string language)
        {
            return language == "ar" ? "البريد الإلكتروني موجود بالفعل لعضو آخر" : "Email already exists for another member.";
        }
        public static string EmailNotValid(string language)
        {
            return language == "ar" ? "البريد الإلكتروني غير صالح" : "Email is not valid.";
        }

        //Mobile Number Validation
        public static string MobileNumberNotFound(string language)
        {
            return language == "ar" ? "لا يوجد رقم موبيل" : "Mobile number not found";
        }
        public static string MobileNumber(string language)
        {
            return language == "ar" ? "رقم الجوال يجب أن يكون رقما" : "Mobile number must be number";
        }
        public static string MobileStartWith(string language)
        {
            return language == "ar" ? "رقم الجوال يجب أن يبدأ بـ 01" : "Mobile Number must start with 01";
        }
        public static string MobileNumberFormat(string language)
        {
            return language == "ar" ? "رقم الجوال يجب أن يكون 11 رقم" : "Mobile Number must be 11 number";
        }

        public static string MobileNumbeExist(string language)
        {
            return language == "ar" ? "رقم الجوال موجود بالفعل لعضو آخر" : "Mobile number already exists for another member.";
        }

        // National Id Validation 
        public static string NationalIdNumber(string language)
        {
            return language == "ar" ? "يجب أن يكون الرقم القومي رقما" : "National Id must be number";
        }
        public static string NationalIdFormat(string language)
        {
            return language == "ar" ? "يجب أن يكون الرقم القومي  14 رقم" : "National Id must be 14 number";
        }
        public static string NationalIdExist(string language)
        {
            return language == "ar" ? "الرقم القومي موجودة بالفعل لعضو آخر" : "National Id already exists for another member.";
        }

        // Photo Validation 
        public static string NoFileUploaded (string language)
        {
            return language == "ar" ? "لم يتم تحميل أي ملف" : "No file uploaded.";
        }
        public static string SizeOfFile(string language)
        {
            return language == "ar" ? "يجب أن يكون حجم الملف أقل من 5 ميجابايت" : "File size must be less than 5 MB.";
        }
        public static string FileExtension(string language)
        {
            return language == "ar" ? "يجب أن ينتهي مسار المجلد بالامتداد .jpg أو .png أو .jpeg" : "Folder Path must end with extension .jpg, .png, or .jpeg";
        }


        // notification 
        public static string NotificationValid(string language)
        {
            return language == "ar" ? "رسالة الاشعار غير صالحة" : "Invalid notification message";
        }
        public static string NotificationToken(string language)
        {
            return language == "ar" ? "لم يتم العثور على Token للأعضاء المحددين" : "No valid tokens found for the specified members";
        }
        public static string NotificationSend(string language)
        {
            return language == "ar" ? "تم إرسال الإشعار بنجاح" : "Notification sent successfully";
        }
        public static string NotificationImage(string language)
        {
            return language == "ar" ? "رابط الصورة غير صحيح" : "Imges not Url";
        }


        // Login 
        public static string PasswordAndIdRequired(string language)
        {
            return language == "ar" ? "ادخلid  وكلمة المرور" : "Id and Password is required";
        }
        public static string InvalidId (string language)
        {
            return language == "ar" ? "رقم التعريف غير صالحة" : "Invalid Id";
        }
        public static string PasswordAndIdIncorrect(string language)
        {
            return language == "ar" ? "بطاقة التعريف او كلمة المرور غير صالحة" : "Id or Password is incorrect";
        }
        public static string AccountDisabled(string language)
        {
            return language == "ar" ? "الحساب معطل" : "Account Disabled";
        }
        public static string LoginSuccessfully(string language)
        {
            return language == "ar" ? "تم تسجيل الدخول بنجاح" : "Login Successfully";
        }

        public static string IncorrectId(string language)
        {
            return language == "ar" ? "رقم المستخدم غير صحيح" : "Member id is incorrect";
        }
        public static string IncorrectOtp(string language)
        {
            return language == "ar" ? "غير صحيح OTP" : "Otp is incorrect";
        }
        public static string DeliveredOtp(string language)
        {
            return language == "ar" ? "تم ارسال رسالة OTP " : "OTP Message delivered";
        }
        public static string PasswordAndConfirmPassword(string language)
        {
            return language == "ar" ? "كلمة المرور غير متساوية" : "Password not Equal ConfirmPasswod";
        }
        public static string ChangePassword(string language)
        {
            return language == "ar" ? "تم" : "Done";
        }


        // MedicalNetwork
        public static string MedicalNetwork(string language)
        {
            return language == "ar" ? "غير موجود" : "Not found";
        }

        // Policy 

        public static string Policy(string language)
        {
            return language == "ar" ? "العميل غير مفعل" : "program id not found";
        }

        // Request 
        public static string ProviderNotFound(string language)
        {
            return language == "ar" ? "مقدم الخدمة غير موجود" : "Provider Id not found";
        }
        public static string EnterProvider(string language)
        {
            return language == "ar" ? "ادخل مقدم الخدمة" : "Enter Provider Id";
        }
        public static string RequestNotFound(string language)
        {
            return language == "ar" ? "لم يتم العثور على الطلب" : "Request not found";
        }


        // Refund

        public static string RefundNotFound(string language)
        {
            return language == "ar" ? "لم يتم العثور على طلب استرداد الأموال" : "Refund  not found";
        }
        public static string AmountNotFound(string language)
        {
            return language == "ar" ? "المبلغ الإجمالي مطلوب" : "Total Amount is required";
        }
        public static string EnterRefund(string language)
        {
            return language == "ar" ? "أدخل معرف الاسترداد" : "Enter Refund id";
        }
        public static string EnterRefundDate(string language)
        {
            return language == "ar" ? "أدخل تاريخ الاسترداد" : "Enter Refund Date";
        }
        public static string RefundDateIncorrect(string language)
        {
            return language == "ar" ? " تاريخ الاسترداد خطأ" : " Refund Date incorrect";
        }
    }
}

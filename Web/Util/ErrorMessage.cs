﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iread_assignment_ms.Web.Util
{
    public class ErrorMessage
    {
        public const String AUDIO_ALREADY_EXIST = "Audio already exist.";
        public const String Attachment_ID_IS_REQUIRED = "Attachment id is required.";
        public const String INVALID_Attachment_ID_VALUE = "Invalid attachment id value.";
        public const String Interaction_ID_IS_REQUIRED = "Interaction id is required.";
        public const String INVALID_Interaction_ID_VALUE = "Invalid interaction id value.";
        public const String INVALID_Comment_ID_VALUE = "Invalid comment id value.";
        public const String Audio_ID_IS_REQUIRED = "Audio id is required.";
        public const String INVALID_Audio_ID_VALUE = "Invalid audio id value.";
        public const String FILE_EXTENSION_NOT_ALLOWED = "File extension is not allowed!.";
        public const String START_DATE_AFTER_NOW = "The start date should be now or after the current date.";
        public const String START_DATE_REQUIRED = "Start date is required.";
        public const String END_DATE_AFTER_NOW = "The end date should be after the current date.";
        public const String END_DATE_REQUIRED = "End date is required.";
        public const String END_DATE_AFTER_START = "The end date should be after the start date.";
        public const String TAG_TITLE_REQUIRED = "Tag title is required.";


        public static List<String> ModelStateParser(ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
        }
    }
}
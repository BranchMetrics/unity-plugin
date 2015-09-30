using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MATSDK
{
    public class MATAdMetadata
    {
        private MATAdGender gender;
        private DateTime? birthDate;
        private bool debugMode;
        private HashSet<string> keywords;
        private float latitude;
        private float longitude;
        private float altitude;
        private Dictionary<string, string> customTargets;

        public MATAdMetadata()
        {
            this.gender = MATAdGender.UNKNOWN;
            this.debugMode = false;
            this.keywords = new HashSet<string>();
            this.customTargets = new Dictionary<string, string>();
        }

        public bool getDebugMode()
        {
            return this.debugMode;
        }

        public void setDebugMode(bool debugMode)
        {
            this.debugMode = debugMode;
        }

        public DateTime? getBirthDate() {
            return this.birthDate;
        }

        public void setBirthDate(DateTime? birthDate)
        {
            this.birthDate = birthDate;
        }

        public void setBirthDate(int year, int month, int day)
        {
            this.birthDate = new DateTime(year, month, day);
        }

        public Dictionary<string, string> getCustomTargets()
        {
            return this.customTargets;
        }

        public void setCustomTargets(Dictionary<string, string> customTargets)
        {
            this.customTargets = customTargets;
        }

        public MATAdGender getGender()
        {
            return this.gender;
        }

        public void setGender(MATAdGender gender)
        {
            this.gender = gender;
        }

        public HashSet<string> getKeywords()
        {
            return this.keywords;
        }

        public void setKeywords(HashSet<string> keywords)
        {
            this.keywords = keywords;
        }

        public void addKeyword(string keyword)
        {
            if (this.keywords == null)
            {
                this.keywords = new HashSet<string>();
            }
            this.keywords.Add(keyword);
        }

        public float getLatitude()
        {
            return this.latitude;
        }

        public float getLongitude()
        {
            return this.longitude;
        }

        public float getAltitude()
        {
        return this.altitude;
        }

        public void setLocation(float latitude, float longitude, float altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }
    }
}

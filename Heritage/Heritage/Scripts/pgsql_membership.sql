--
-- TOC entry 127 (class 1259 OID 86037)
-- Dependencies: 6
-- Name: ProfileData; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "ProfileData" (
    "pId" character(36) NOT NULL,
    "Profile" character(36) NOT NULL,
    "Name" character varying(255) NOT NULL,
    "ValueString" text,
    "ValueBinary" bytea
);


--
-- TOC entry 128 (class 1259 OID 86043)
-- Dependencies: 6
-- Name: Profiles; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "Profiles" (
    "pId" character(36) NOT NULL,
    "Username" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "IsAnonymous" boolean,
    "LastActivityDate" timestamp with time zone,
    "LastUpdatedDate" timestamp with time zone
);


--
-- TOC entry 129 (class 1259 OID 86049)
-- Dependencies: 6
-- Name: Roles; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "Roles" (
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


--
-- TOC entry 130 (class 1259 OID 86055)
-- Dependencies: 6
-- Name: Sessions; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "Sessions" (
    "SessionId" character varying(80) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "Created" timestamp with time zone NOT NULL,
    "Expires" timestamp with time zone NOT NULL,
    "Timeout" integer NOT NULL,
    "Locked" boolean NOT NULL,
    "LockId" integer NOT NULL,
    "LockDate" timestamp with time zone NOT NULL,
    "Data" text,
    "Flags" integer NOT NULL
);


--
-- TOC entry 131 (class 1259 OID 86061)
-- Dependencies: 6
-- Name: Users; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "Users" (
    "pId" character(36) NOT NULL,
    "Username" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "Email" character varying(128),
    "Comment" character varying(128),
    "Password" character varying(255) NOT NULL,
    "PasswordQuestion" character varying(255),
    "PasswordAnswer" character varying(255),
    "IsApproved" boolean,
    "LastActivityDate" timestamp with time zone,
    "LastLoginDate" timestamp with time zone,
    "LastPasswordChangedDate" timestamp with time zone,
    "CreationDate" timestamp with time zone,
    "IsOnLine" boolean,
    "IsLockedOut" boolean,
    "LastLockedOutDate" timestamp with time zone,
    "FailedPasswordAttemptCount" integer,
    "FailedPasswordAttemptWindowStart" timestamp with time zone,
    "FailedPasswordAnswerAttemptCount" integer,
    "FailedPasswordAnswerAttemptWindowStart" timestamp with time zone
);


--
-- TOC entry 132 (class 1259 OID 86067)
-- Dependencies: 6
-- Name: UsersInRoles; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE "UsersInRoles" (
    "Username" character varying(255) NOT NULL,
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


--
-- TOC entry 1755 (class 2606 OID 86074)
-- Dependencies: 127 127
-- Name: profiledata_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_pkey PRIMARY KEY ("pId");


--
-- TOC entry 1757 (class 2606 OID 86076)
-- Dependencies: 127 127 127
-- Name: profiledata_profile_name_unique; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_name_unique UNIQUE ("Profile", "Name");


--
-- TOC entry 1760 (class 2606 OID 86078)
-- Dependencies: 128 128
-- Name: profiles_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_pkey PRIMARY KEY ("pId");


--
-- TOC entry 1762 (class 2606 OID 86080)
-- Dependencies: 128 128 128
-- Name: profiles_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 1764 (class 2606 OID 86082)
-- Dependencies: 129 129 129
-- Name: roles_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Roles"
    ADD CONSTRAINT roles_pkey PRIMARY KEY ("Rolename", "ApplicationName");


--
-- TOC entry 1766 (class 2606 OID 86084)
-- Dependencies: 130 130 130
-- Name: sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT sessions_pkey PRIMARY KEY ("SessionId", "ApplicationName");


--
-- TOC entry 1770 (class 2606 OID 86086)
-- Dependencies: 131 131
-- Name: users_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT users_pkey PRIMARY KEY ("pId");


--
-- TOC entry 1772 (class 2606 OID 86088)
-- Dependencies: 131 131 131
-- Name: users_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT users_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 1774 (class 2606 OID 86090)
-- Dependencies: 132 132 132 132
-- Name: usersinroles_pkey; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_pkey PRIMARY KEY ("Username", "Rolename", "ApplicationName");


--
-- TOC entry 1758 (class 1259 OID 86091)
-- Dependencies: 128
-- Name: profiles_isanonymous_index; Type: INDEX; Schema: public; Owner: -; Tablespace: 
--

CREATE INDEX profiles_isanonymous_index ON "Profiles" USING btree ("IsAnonymous");


--
-- TOC entry 1767 (class 1259 OID 86092)
-- Dependencies: 131
-- Name: users_email_index; Type: INDEX; Schema: public; Owner: -; Tablespace: 
--

CREATE INDEX users_email_index ON "Users" USING btree ("Email");


--
-- TOC entry 1768 (class 1259 OID 86093)
-- Dependencies: 131
-- Name: users_islockedout_index; Type: INDEX; Schema: public; Owner: -; Tablespace: 
--

CREATE INDEX users_islockedout_index ON "Users" USING btree ("IsLockedOut");


--
-- TOC entry 1775 (class 2606 OID 86094)
-- Dependencies: 1759 127 128
-- Name: profiledata_profile_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_fkey FOREIGN KEY ("Profile") REFERENCES "Profiles"("pId") ON DELETE CASCADE;


--
-- TOC entry 1776 (class 2606 OID 86099)
-- Dependencies: 128 1771 131 131 128
-- Name: profiles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 1777 (class 2606 OID 86104)
-- Dependencies: 129 1763 129 132 132
-- Name: usersinroles_rolename_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_rolename_fkey FOREIGN KEY ("Rolename", "ApplicationName") REFERENCES "Roles"("Rolename", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 1778 (class 2606 OID 86109)
-- Dependencies: 131 131 1771 132 132
-- Name: usersinroles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
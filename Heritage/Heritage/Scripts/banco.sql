--
-- PostgreSQL database dump
--

-- Dumped from database version 8.3.7
-- Dumped by pg_dump version 9.1.3
-- Started on 2012-05-14 19:57:07

SET statement_timeout = 0;
SET client_encoding = 'WIN1252';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 134 (class 1259 OID 24713)
-- Dependencies: 3
-- Name: banco_administradores; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_administradores (
    codigo bigint NOT NULL,
    login character varying(255) NOT NULL,
    nome character varying(255) NOT NULL
);


ALTER TABLE public.banco_administradores OWNER TO postgres;

--
-- TOC entry 133 (class 1259 OID 24711)
-- Dependencies: 3 134
-- Name: banco_administradores_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_administradores_codigo_seq
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_administradores_codigo_seq OWNER TO postgres;

--
-- TOC entry 1812 (class 0 OID 0)
-- Dependencies: 133
-- Name: banco_administradores_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_administradores_codigo_seq OWNED BY banco_administradores.codigo;


--
-- TOC entry 136 (class 1259 OID 24721)
-- Dependencies: 3
-- Name: banco_agencias; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_agencias (
    codigo bigint NOT NULL,
    numero integer NOT NULL,
    dv integer NOT NULL,
    nome character varying(255) NOT NULL,
    data_criacao timestamp without time zone NOT NULL,
    endereco bigint NOT NULL,
    responsavel bigint NOT NULL
);


ALTER TABLE public.banco_agencias OWNER TO postgres;

--
-- TOC entry 135 (class 1259 OID 24719)
-- Dependencies: 3 136
-- Name: banco_agencias_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_agencias_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_agencias_codigo_seq OWNER TO postgres;

--
-- TOC entry 1813 (class 0 OID 0)
-- Dependencies: 135
-- Name: banco_agencias_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_agencias_codigo_seq OWNED BY banco_agencias.codigo;


--
-- TOC entry 138 (class 1259 OID 24729)
-- Dependencies: 3
-- Name: banco_clientes; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_clientes (
    codigo bigint NOT NULL,
    login character varying(255) NOT NULL,
    nome character varying(255) NOT NULL,
    endereco bigint NOT NULL
);


ALTER TABLE public.banco_clientes OWNER TO postgres;

--
-- TOC entry 137 (class 1259 OID 24727)
-- Dependencies: 3 138
-- Name: banco_clientes_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_clientes_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_clientes_codigo_seq OWNER TO postgres;

--
-- TOC entry 1814 (class 0 OID 0)
-- Dependencies: 137
-- Name: banco_clientes_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_clientes_codigo_seq OWNED BY banco_clientes.codigo;


--
-- TOC entry 140 (class 1259 OID 24737)
-- Dependencies: 3
-- Name: banco_contas_correntes; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_contas_correntes (
    codigo bigint NOT NULL,
    numero integer NOT NULL,
    dv integer NOT NULL,
    data_abertura timestamp without time zone NOT NULL,
    data_encerramento timestamp without time zone,
    gerente bigint NOT NULL,
    agencia bigint NOT NULL,
    cliente bigint
);


ALTER TABLE public.banco_contas_correntes OWNER TO postgres;

--
-- TOC entry 139 (class 1259 OID 24735)
-- Dependencies: 140 3
-- Name: banco_contas_correntes_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_contas_correntes_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_contas_correntes_codigo_seq OWNER TO postgres;

--
-- TOC entry 1815 (class 0 OID 0)
-- Dependencies: 139
-- Name: banco_contas_correntes_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_contas_correntes_codigo_seq OWNED BY banco_contas_correntes.codigo;


--
-- TOC entry 142 (class 1259 OID 24745)
-- Dependencies: 3
-- Name: banco_enderecos; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_enderecos (
    codigo bigint NOT NULL,
    logradouro character varying(255) NOT NULL,
    numero character varying(255) NOT NULL,
    complemento character varying(255),
    cep character varying(255) NOT NULL,
    bairro character varying(255) NOT NULL,
    cidade character varying(255) NOT NULL,
    estado character varying(255) NOT NULL
);


ALTER TABLE public.banco_enderecos OWNER TO postgres;

--
-- TOC entry 141 (class 1259 OID 24743)
-- Dependencies: 3 142
-- Name: banco_enderecos_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_enderecos_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_enderecos_codigo_seq OWNER TO postgres;

--
-- TOC entry 1816 (class 0 OID 0)
-- Dependencies: 141
-- Name: banco_enderecos_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_enderecos_codigo_seq OWNED BY banco_enderecos.codigo;


--
-- TOC entry 144 (class 1259 OID 24753)
-- Dependencies: 3
-- Name: banco_gerentes; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_gerentes (
    codigo bigint NOT NULL,
    login character varying(255) NOT NULL,
    nome character varying(255) NOT NULL,
    agencia bigint NOT NULL
);


ALTER TABLE public.banco_gerentes OWNER TO postgres;

--
-- TOC entry 143 (class 1259 OID 24751)
-- Dependencies: 144 3
-- Name: banco_gerentes_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_gerentes_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_gerentes_codigo_seq OWNER TO postgres;

--
-- TOC entry 1817 (class 0 OID 0)
-- Dependencies: 143
-- Name: banco_gerentes_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_gerentes_codigo_seq OWNED BY banco_gerentes.codigo;


--
-- TOC entry 146 (class 1259 OID 24761)
-- Dependencies: 3
-- Name: banco_operacoes_financeiras; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE banco_operacoes_financeiras (
    codigo bigint NOT NULL,
    data timestamp without time zone NOT NULL,
    valor numeric(19,5) NOT NULL,
    descricao character varying(255) NOT NULL,
    conta bigint NOT NULL
);


ALTER TABLE public.banco_operacoes_financeiras OWNER TO postgres;

--
-- TOC entry 145 (class 1259 OID 24759)
-- Dependencies: 3 146
-- Name: banco_operacoes_financeiras_codigo_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE banco_operacoes_financeiras_codigo_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.banco_operacoes_financeiras_codigo_seq OWNER TO postgres;

--
-- TOC entry 1818 (class 0 OID 0)
-- Dependencies: 145
-- Name: banco_operacoes_financeiras_codigo_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE banco_operacoes_financeiras_codigo_seq OWNED BY banco_operacoes_financeiras.codigo;


--
-- TOC entry 1781 (class 2604 OID 24716)
-- Dependencies: 133 134 134
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_administradores ALTER COLUMN codigo SET DEFAULT nextval('banco_administradores_codigo_seq'::regclass);


--
-- TOC entry 1782 (class 2604 OID 24724)
-- Dependencies: 135 136 136
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_agencias ALTER COLUMN codigo SET DEFAULT nextval('banco_agencias_codigo_seq'::regclass);


--
-- TOC entry 1783 (class 2604 OID 24732)
-- Dependencies: 137 138 138
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_clientes ALTER COLUMN codigo SET DEFAULT nextval('banco_clientes_codigo_seq'::regclass);


--
-- TOC entry 1784 (class 2604 OID 24740)
-- Dependencies: 139 140 140
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_contas_correntes ALTER COLUMN codigo SET DEFAULT nextval('banco_contas_correntes_codigo_seq'::regclass);


--
-- TOC entry 1785 (class 2604 OID 24748)
-- Dependencies: 141 142 142
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_enderecos ALTER COLUMN codigo SET DEFAULT nextval('banco_enderecos_codigo_seq'::regclass);


--
-- TOC entry 1786 (class 2604 OID 24756)
-- Dependencies: 144 143 144
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_gerentes ALTER COLUMN codigo SET DEFAULT nextval('banco_gerentes_codigo_seq'::regclass);


--
-- TOC entry 1787 (class 2604 OID 24764)
-- Dependencies: 146 145 146
-- Name: codigo; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_operacoes_financeiras ALTER COLUMN codigo SET DEFAULT nextval('banco_operacoes_financeiras_codigo_seq'::regclass);


--
-- TOC entry 1789 (class 2606 OID 24718)
-- Dependencies: 134 134
-- Name: banco_administradores_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_administradores
    ADD CONSTRAINT banco_administradores_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1791 (class 2606 OID 24726)
-- Dependencies: 136 136
-- Name: banco_agencias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_agencias
    ADD CONSTRAINT banco_agencias_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1793 (class 2606 OID 24734)
-- Dependencies: 138 138
-- Name: banco_clientes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_clientes
    ADD CONSTRAINT banco_clientes_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1795 (class 2606 OID 24742)
-- Dependencies: 140 140
-- Name: banco_contas_correntes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_contas_correntes
    ADD CONSTRAINT banco_contas_correntes_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1797 (class 2606 OID 24750)
-- Dependencies: 142 142
-- Name: banco_enderecos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_enderecos
    ADD CONSTRAINT banco_enderecos_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1799 (class 2606 OID 24758)
-- Dependencies: 144 144
-- Name: banco_gerentes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_gerentes
    ADD CONSTRAINT banco_gerentes_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1801 (class 2606 OID 24766)
-- Dependencies: 146 146
-- Name: banco_operacoes_financeiras_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY banco_operacoes_financeiras
    ADD CONSTRAINT banco_operacoes_financeiras_pkey PRIMARY KEY (codigo);


--
-- TOC entry 1807 (class 2606 OID 24792)
-- Dependencies: 140 1792 138
-- Name: fk86e54ad9711c5b7; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_contas_correntes
    ADD CONSTRAINT fk86e54ad9711c5b7 FOREIGN KEY (cliente) REFERENCES banco_clientes(codigo);


--
-- TOC entry 1803 (class 2606 OID 24772)
-- Dependencies: 136 1788 134
-- Name: fk_agencia_administrador; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_agencias
    ADD CONSTRAINT fk_agencia_administrador FOREIGN KEY (responsavel) REFERENCES banco_administradores(codigo);


--
-- TOC entry 1802 (class 2606 OID 24767)
-- Dependencies: 1796 142 136
-- Name: fk_agencia_endereco; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_agencias
    ADD CONSTRAINT fk_agencia_endereco FOREIGN KEY (endereco) REFERENCES banco_enderecos(codigo);


--
-- TOC entry 1804 (class 2606 OID 24777)
-- Dependencies: 1796 142 138
-- Name: fk_cliente_endereco; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_clientes
    ADD CONSTRAINT fk_cliente_endereco FOREIGN KEY (endereco) REFERENCES banco_enderecos(codigo);


--
-- TOC entry 1806 (class 2606 OID 24787)
-- Dependencies: 1790 136 140
-- Name: fk_conta_agencia; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_contas_correntes
    ADD CONSTRAINT fk_conta_agencia FOREIGN KEY (agencia) REFERENCES banco_agencias(codigo);


--
-- TOC entry 1805 (class 2606 OID 24782)
-- Dependencies: 1798 144 140
-- Name: fk_conta_gerente; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_contas_correntes
    ADD CONSTRAINT fk_conta_gerente FOREIGN KEY (gerente) REFERENCES banco_gerentes(codigo);


--
-- TOC entry 1808 (class 2606 OID 24797)
-- Dependencies: 136 1790 144
-- Name: fk_gerente_agencia; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_gerentes
    ADD CONSTRAINT fk_gerente_agencia FOREIGN KEY (agencia) REFERENCES banco_agencias(codigo);


--
-- TOC entry 1809 (class 2606 OID 24802)
-- Dependencies: 146 140 1794
-- Name: fk_operacao_conta; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY banco_operacoes_financeiras
    ADD CONSTRAINT fk_operacao_conta FOREIGN KEY (conta) REFERENCES banco_contas_correntes(codigo);


-- Completed on 2012-05-14 19:57:08

--
-- PostgreSQL database dump complete
--


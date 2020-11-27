using System;
using System.Linq;
/**

@auth kevin-chen@foxmail.com
@date 2020.11.27
@github https://github.com/kevinchen2046

Example:
原生用法:
            byte[] sbytes = new byte[]{(byte)255};
            byte[] dbytes = BitConverter.GetBytes((double)2.45666);
            byte[] fbytes = BitConverter.GetBytes((float)1.4566236f);
            byte[] ibytes = BitConverter.GetBytes((int)156);
            byte[] strbytes = System.Text.Encoding.Default.GetBytes("abc");
            byte[] ndata = MeagByte(sbytes,dbytes, fbytes, ibytes,BitConverter.GetBytes((int)strbytes.Length),strbytes);
            int p=0;
            int s=(int)ndata[0];
            p++;
            double d = BitConverter.ToDouble(ndata, p);
            p+=Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT64);
            float f = BitConverter.ToSingle(ndata, p);
            p+=Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT32);
            int i = BitConverter.ToInt32(ndata, p);
            p+=Convert.ToInt32(ByteArraySize.SIZE_OF_INT32);
            int strlen = BitConverter.ToInt32(ndata, p);
            p+=Convert.ToInt32(ByteArraySize.SIZE_OF_INT32);

            byte[] strbyte=Sub(ndata,p,strlen);
            string str = System.Text.Encoding.Default.GetString(strbyte);

            Logger.Log(s,d,f,i,str);
ByteArray用法:
            ByteArray bytes=new ByteArray();
            bytes.WriteByte(212);
            bytes.WriteDouble(5.34245121);
            bytes.WriteFloat(6.34245f);
            bytes.WriteInt(563214);
            bytes.WriteUTF("hellp!!!!");
            bytes.WriteBoolean(false);
            int len=bytes.WriteUTFBytes("some gays!!");

            bytes.Position=0;
            Logger.Log(bytes.ReadByte(),bytes.ReadDouble(),bytes.ReadFloat(),bytes.ReadInt(),bytes.ReadUTF(),bytes.ReadBoolean(),bytes.ReadUTFBytes(len));

todo:
    BIG_ENDIAN 待实现
    ByteArray 的byte[]写入重构次数可以优化为一次
*/
namespace vitamin
{
    /**
     * Endian 类中包含一些值，它们表示用于表示多字节数字的字节顺序。
     * 字节顺序为 bigEndian（最高有效字节位于最前）或 littleEndian（最低有效字节位于最前）。
     * @version 1.0
     * @platform Native
     * @language zh_CN
     */
    public class ByteArrayEndian
    {
        /**
         * 表示多字节数字的最低有效字节位于字节序列的最前面。
         * 十六进制数字 0x12345678 包含 4 个字节（每个字节包含 2 个十六进制数字）。最高有效字节为 0x12。最低有效字节为 0x78。（对于等效的十进制数字 305419896，最高有效数字是 3，最低有效数字是 6）。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public static string LITTLE_ENDIAN = "littleEndian";

        /**
         * 表示多字节数字的最高有效字节位于字节序列的最前面。
         * 十六进制数字 0x12345678 包含 4 个字节（每个字节包含 2 个十六进制数字）。最高有效字节为 0x12。最低有效字节为 0x78。（对于等效的十进制数字 305419896，最高有效数字是 3，最低有效数字是 6）。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public static string BIG_ENDIAN = "bigEndian";

    }

    public enum EndianConst
    {
        LITTLE_ENDIAN = 0,
        BIG_ENDIAN = 1
    }

    public enum ByteArraySize
    {

        SIZE_OF_BOOLEAN = 1,

        SIZE_OF_INT8 = 1,

        SIZE_OF_INT16 = 2,

        SIZE_OF_INT32 = 4,

        SIZE_OF_UINT8 = 1,

        SIZE_OF_UINT16 = 2,

        SIZE_OF_UINT32 = 4,

        SIZE_OF_FLOAT32 = 4,

        SIZE_OF_FLOAT64 = 8
    }
    /**
     * ByteArray 类提供用于优化读取、写入以及处理二进制数据的方法和属性。
     * 注意：ByteArray 类适用于需要在字节层访问数据的高级开发人员。
     * @version 1.0
     * @platform Native
     * @includeExample egret/utils/ByteArray.ts
     * @language zh_CN
     */
    public class ByteArray
    {
        protected byte[] _bytes;
        /**
         * @private
         */
        protected int _position;

        /**
         * 
         * 已经使用的字节偏移量
         * @protected
         * @type {number}
         * @memberOf ByteArray
         */
        protected int write_position;

        protected EndianConst __endian;
        /**
         * 更改或读取数据的字节顺序；egret.EndianConst.BIG_ENDIAN 或 egret.EndianConst.LITTLE_ENDIAN。
         * @default egret.EndianConst.BIG_ENDIAN
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public string Endian
        {
            set { this.__endian = value == ByteArrayEndian.LITTLE_ENDIAN ? EndianConst.LITTLE_ENDIAN : EndianConst.BIG_ENDIAN; }
            get { return this.__endian == EndianConst.LITTLE_ENDIAN ? ByteArrayEndian.LITTLE_ENDIAN : ByteArrayEndian.BIG_ENDIAN; }
        }

        /**
         * @version 1.0
         * @platform Native
         */
        public ByteArray(byte[] bytes = null)
        {
            this.write_position = 0;
            this._position = 0;
            this._bytes = bytes;
            this.Endian = ByteArrayEndian.LITTLE_ENDIAN;
        }

        /**
         * 可读的剩余字节数
         * 
         * @returns 
         * 
         * @memberOf ByteArray
         */
        public int ReadAvailable
        {
            get { return this.write_position - this._position; }
        }

        public byte[] Bytes
        {
            get
            {
                return this._bytes;
            }
        }
        public byte[] bytesCopy()
        {
            byte[] data = new byte[this._bytes.Length];
            this._bytes.CopyTo(data, 0);
            return data;
        }

        /**
         * The current position of the file pointer (in bytes) to move or return to the ByteArray object. The next time you start reading reading method call in this position, or will start writing in this position next time call a write method.
         * @version 1.0
         * @platform Native
         * @language en_US
         */
        /**
         * 将文件指针的当前位置（以字节为单位）移动或返回到 ByteArray 对象中。下一次调用读取方法时将在此位置开始读取，或者下一次调用写入方法时将在此位置开始写入。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int Position
        {
            set
            {
                this._position = value;
                if (value > this.write_position)
                {
                    this.write_position = value;
                }
            }
            get { return this._position; }
        }

        /**
         * ByteArray 对象的长度（以字节为单位）。
         * 如果将长度设置为大于当前长度的值，则用零填充字节数组的右侧。
         * 如果将长度设置为小于当前长度的值，将会截断该字节数组。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int Length
        {
            set
            {
                this.write_position = value;
                if (this._bytes.Length > value)
                {
                    this._position = value;
                }
                this._validateBuffer(value);
            }
            get { return this.write_position; }
        }

        protected void _validateBuffer(int value)
        {
            if (this._bytes == null)
            {
                this._bytes = new byte[value];
                return;
            }
            if (this._bytes.Length > value)
            {
                this._bytes = this._bytes.Skip(0).Take(value).ToArray();
                return;
            }
            if (this._bytes.Length < value)
            {
                byte[] data = new byte[value];
                this._bytes.CopyTo(data, 0);
                this._bytes = data;
            }
        }

        /**
         * 可从字节数组的当前位置到数组末尾读取的数据的字节数。
         * 每次访问 ByteArray 对象时，将 bytesAvailable 属性与读取方法结合使用，以确保读取有效的数据。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int BytesAvailable
        {
            get { return this._bytes.Length - this._position; }
        }

        /**
         * 清除字节数组的内容，并将 length 和 position 属性重置为 0。
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void Clear()
        {
            this._bytes = null;
            this._position = 0;
            this.write_position = 0;
        }

        /**
         * 从字节流中读取布尔值。读取单个字节，如果字节非零，则返回 true，否则返回 false
         * @return 如果字节不为零，则返回 true，否则返回 false
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public bool ReadBoolean()
        {
            bool result = false;
            if (this.Validate(Convert.ToInt32(ByteArraySize.SIZE_OF_BOOLEAN)))
            {
                result = BitConverter.ToBoolean(this._bytes, this.Position);
                this.Position++;
            }
            return result;
        }

        /**
         * 从字节流中读取带符号的字节
         * @return 介于 -128 和 127 之间的整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int ReadByte()
        {
            int result = 0;
            if (this.Validate(Convert.ToInt32(ByteArraySize.SIZE_OF_INT8)))
            {
                result = (int)this._bytes[this.Position];
                this.Position++;
            }
            return result;
        }

        /**
         * 从字节流中读取 length 参数指定的数据字节数。从 offset 指定的位置开始，将字节读入 bytes 参数指定的 ByteArray 对象中，并将字节写入目标 ByteArray 中
         * @param bytes 要将数据读入的 ByteArray 对象
         * @param offset bytes 中的偏移（位置），应从该位置写入读取的数据
         * @param length 要读取的字节数。默认值 0 导致读取所有可用的数据
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void ReadBytes(ByteArray bytes, int offset = 0, int length = 0)
        {
            if (bytes == null)
            {
                return;
            }
            int pos = this._position;
            int available = this.write_position - pos;
            if (available < 0)
            {
                Logger.Error("ByteArray Available < 0");
                return;
            }
            if (length == 0)
            {
                length = available;
            }
            else if (length > available)
            {
                Logger.Error("ByteArray length > Available");
                return;
            }
            int position = bytes._position;
            bytes._position = 0;
            bytes.validateBuffer(offset + length);
            bytes._position = position;
            this._bytes.Skip(0).Take(length).ToArray().CopyTo(bytes._bytes, offset);
            this.Position += length;
        }

        /**
         * 从字节流中读取一个 IEEE 754 双精度（64 位）浮点数
         * @return 双精度（64 位）浮点数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public double ReadDouble()
        {
            double result = 0f;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT64);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToDouble(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToDouble(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个 IEEE 754 单精度（32 位）浮点数
         * @return 单精度（32 位）浮点数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public float ReadFloat()
        {
            float result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT32);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToSingle(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToSingle(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个带符号的 32 位整数
         * @return 介于 -2147483648 和 2147483647 之间的 32 位带符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int ReadInt()
        {
            int result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_INT32);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToInt32(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToInt32(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个带符号的 16 位整数
         * @return 介于 -32768 和 32767 之间的 16 位带符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int ReadShort()
        {
            int result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_INT16);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToInt16(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToInt16(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取无符号的字节
         * @return 介于 0 和 255 之间的无符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public uint ReadUnsignedByte()
        {
            uint result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT8);
            if (this.Validate(len))
            {
                result = (uint)this._bytes[this._position];
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个无符号的 32 位整数
         * @return 介于 0 和 4294967295 之间的 32 位无符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public uint ReadUnsignedInt()
        {
            uint result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT32);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToUInt32(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToUInt32(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个无符号的 16 位整数
         * @return 介于 0 和 65535 之间的 16 位无符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public uint ReadUnsignedShort()
        {
            uint result = 0;
            int len = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT16);
            if (this.Validate(len))
            {
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = BitConverter.ToUInt16(this._bytes.Skip(this._position).Take(len).Reverse().ToArray(), 0);
                }
                else
                {
                    result = BitConverter.ToUInt16(this._bytes, this._position);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个 UTF-8 字符串。假定字符串的前缀是无符号的短整型（以字节表示长度）
         * @return UTF-8 编码的字符串
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public string ReadUTF()
        {
            string result = "";
            int len = (int)this.ReadUnsignedShort();
            if (len == 0) return result;
            if (this.Validate(len))
            {
                byte[] strbyte = this._bytes.Skip(this._position).Take(len).ToArray();
                if (this.__endian == EndianConst.BIG_ENDIAN)
                {
                    result = this.decodeUTF8(strbyte.Reverse().ToArray());
                }
                else
                {
                    result = this.decodeUTF8(strbyte);
                }
                this.Position += len;
            }
            return result;
        }

        /**
         * 从字节流中读取一个由 length 参数指定的 UTF-8 字节序列，并返回一个字符串
         * @param length 指明 UTF-8 字节长度的无符号短整型数
         * @return 由指定长度的 UTF-8 字节组成的字符串
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public string ReadUTFBytes(int length)
        {
            string result = "";
            if (!this.Validate(length))
            {
                return result;
            }
            byte[] strbyte = this._bytes.Skip(this._position).Take(length).ToArray();
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                result = this.decodeUTF8(strbyte.Reverse().ToArray());
            }
            else
            {
                result = this.decodeUTF8(strbyte);
            }
            this.Position += length;
            return result;
        }

        /**
         * 写入布尔值。根据 value 参数写入单个字节。如果为 true，则写入 1，如果为 false，则写入 0
         * @param value 确定写入哪个字节的布尔值。如果该参数为 true，则该方法写入 1；如果该参数为 false，则该方法写入 0
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteBoolean(bool value)
        {
            this.validateBuffer(Convert.ToInt32(ByteArraySize.SIZE_OF_BOOLEAN));
            this._bytes[this.Position++] = (byte)(value == true ? 1 : 0);
        }

        /**
         * 在字节流中写入一个字节
         * 使用参数的低 8 位。忽略高 24 位
         * @param value 一个 32 位整数。低 8 位将被写入字节流
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteByte(int value)
        {
            this.validateBuffer(Convert.ToInt32(ByteArraySize.SIZE_OF_INT8));
            this._bytes[this.Position++] = (byte)(value & 0xff);
        }

        /**
         * 将指定字节数组 bytes（起始偏移量为 offset，从零开始的索引）中包含 length 个字节的字节序列写入字节流
         * 如果省略 length 参数，则使用默认长度 0；该方法将从 offset 开始写入整个缓冲区。如果还省略了 offset 参数，则写入整个缓冲区
         * 如果 offset 或 length 超出范围，它们将被锁定到 bytes 数组的开头和结尾
         * @param bytes ByteArray 对象
         * @param offset 从 0 开始的索引，表示在数组中开始写入的位置
         * @param length 一个无符号整数，表示在缓冲区中的写入范围
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteBytes(ByteArray bytes, int offset = 0, int length = 0)
        {
            int writeLength;
            if (offset < 0)
            {
                return;
            }
            if (length < 0)
            {
                return;
            }
            else if (length == 0)
            {
                writeLength = bytes.Length - offset;
            }
            else
            {
                writeLength = Math.Min(bytes.Length - offset, length);
            }
            if (writeLength > 0)
            {
                this.validateBuffer(writeLength);
                bytes._bytes.Skip(offset).Take(writeLength).ToArray().CopyTo(this._bytes, this._position);
                // this._bytes.set(bytes._bytes.subarray(offset, offset + writeLength), this._position);
                this.Position = this._position + writeLength;
            }
        }

        /**
         * 在字节流中写入一个 IEEE 754 双精度（64 位）浮点数
         * @param value 双精度（64 位）浮点数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteDouble(double value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT64);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            //this.data.setFloat64(this._position, value, this.__endian == EndianConst.LITTLE_ENDIAN);
            this.Position += bytelen;
        }

        /**
         * 在字节流中写入一个 IEEE 754 单精度（32 位）浮点数
         * @param value 单精度（32 位）浮点数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteFloat(float value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_FLOAT32);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += bytelen;
        }

        /**
         * 在字节流中写入一个带符号的 32 位整数
         * @param value 要写入字节流的整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteInt(int value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_INT32);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += bytelen;
        }

        /**
         * 在字节流中写入一个 16 位整数。使用参数的低 16 位。忽略高 16 位
         * @param value 32 位整数，该整数的低 16 位将被写入字节流
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteShort(int value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_INT16);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += bytelen;
        }

        /**
         * 在字节流中写入一个无符号的 32 位整数
         * @param value 要写入字节流的无符号整数
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteUnsignedInt(uint value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT32);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += bytelen;
        }

        /**
         * 在字节流中写入一个无符号的 16 位整数
         * @param value 要写入字节流的无符号整数
         * @version Egret 2.5
         * @platform Native
         * @language zh_CN
         */
        public void WriteUnsignedShort(uint value)
        {
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT16);
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += bytelen;
        }

        /**
         * 将 UTF-8 字符串写入字节流。先写入以字节表示的 UTF-8 字符串长度（作为 16 位整数），然后写入表示字符串字符的字节
         * @param value 要写入的字符串值
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public void WriteUTF(string value)
        {
            byte[] utf8bytes = this.encodeUTF8(value);
            int length = utf8bytes.Length;
            int bytelen = Convert.ToInt32(ByteArraySize.SIZE_OF_UINT16) + length;
            this.validateBuffer(bytelen);
            byte[] bytes = BitConverter.GetBytes(length);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                bytes.Reverse().ToArray().CopyTo(this._bytes, this._position);
            }
            else
            {
                bytes.CopyTo(this._bytes, this._position);
            }
            this.Position += Convert.ToInt32(ByteArraySize.SIZE_OF_UINT16);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                this._writeBytes(utf8bytes.Reverse().ToArray(), false);
            }
            else
            {
                this._writeBytes(utf8bytes, false);
            }
        }

        /**
         * 将 UTF-8 字符串写入字节流。类似于 writeUTF() 方法，但 writeUTFBytes() 不使用 16 位长度的词为字符串添加前缀
         * @param value 要写入的字符串值
         * @version 1.0
         * @platform Native
         * @language zh_CN
         */
        public int WriteUTFBytes(string value)
        {
            byte[] utf8bytes = this.encodeUTF8(value);
            if (this.__endian == EndianConst.BIG_ENDIAN)
            {
                return this._writeBytes(utf8bytes.Reverse().ToArray(), false);
            }
            else
            {
                return this._writeBytes(utf8bytes, false);
            }
        }

        /**
         *
         * @returns
         * @version 1.0
         * @platform Native
         */
        override public string ToString()
        {
            return "[ByteArray] length:" + this.Length + ", bytesAvailable:" + this.BytesAvailable+"\n"+CollectionUtil.Join(this._bytes," ");
        }

        /**
         * @private
         * 将 Uint8Array 写入字节流
         * @param bytes 要写入的Uint8Array
         * @param validateBuffer
         */
        public int _writeBytes(byte[] bytes, bool validateBuffer = true)
        {
            int pos = this._position;
            int npos = pos + bytes.Length;
            if (validateBuffer)
            {
                this.validateBuffer(npos);
            }
            bytes.CopyTo(this._bytes, pos);
            this.Position = npos;
            return bytes.Length;
        }

        /**
         * @param len
         * @returns
         * @version 1.0
         * @platform Native
         * @private
         */
        public bool Validate(int len)
        {
            int bl = this._bytes.Length;
            if (bl > 0 && this._position + len <= bl)
            {
                return true;
            }
            else
            {
                Logger.Error("bytearray validate fail!");
            }
            return false;
        }

        /**********************/
        /*  PRIVATE METHODS   */
        /**********************/
        /**
         * @private
         * @param len
         * @param needReplace
         */
        protected void validateBuffer(int len)
        {
            this.write_position = len > this.write_position ? len : this.write_position;
            len += this._position;
            this._validateBuffer(len);
        }

        /**
         * @private
         * UTF-8 Encoding/Decoding
         */
        private byte[] encodeUTF8(string str)
        {
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            byte[] bytes = utf8.GetBytes(str);
            return bytes;
        }

        /**
         * @private
         *
         * @param data
         * @returns
         */
        private string decodeUTF8(byte[] bytes)
        {
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            return utf8.GetString(bytes);
        }

        /**
         * @private
         */
        private int EOF_byte = -1;
        /**
         * @private
         */
        private int EOF_code_point = -1;

        /**
         * @private
         *
         * @param a
         * @param min
         * @param max
         */
        private bool inRange(int a, int min, int max)
        {
            return min <= a && a <= max;
        }

        /**
         * @private
         *
         * @param string
         */
        private int[] stringToCodePoints(string str)
        {
            System.Collections.Generic.List<int> cps = new System.Collections.Generic.List<int>();
            char[] chars = str.ToCharArray();
            int i = 0, n = chars.Length;

            while (i < chars.Length)
            {
                int c = chars[i];
                if (!this.inRange(c, 0xD800, 0xDFFF))
                {
                    cps.Add(c);
                }
                else if (this.inRange(c, 0xDC00, 0xDFFF))
                {
                    cps.Add(0xFFFD);
                }
                else
                { // (inRange(c, 0xD800, 0xDBFF))
                    if (i == n - 1)
                    {
                        cps.Add(0xFFFD);
                    }
                    else
                    {
                        int d = chars[i + 1];
                        if (this.inRange(d, 0xDC00, 0xDFFF))
                        {
                            int a = c & 0x3FF;
                            int b = d & 0x3FF;
                            i += 1;
                            cps.Add(0x10000 + (a << 10) + b);
                        }
                        else
                        {
                            cps.Add(0xFFFD);
                        }
                    }
                }
                i += 1;
            }
            return cps.ToArray();
        }
    }
}
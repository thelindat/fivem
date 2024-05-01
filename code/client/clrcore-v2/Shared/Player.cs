#if IS_FXSERVER
using System;
#endif
using CitizenFX.Core;

namespace CitizenFX.Shared
{
#if IS_FXSERVER
	public abstract class Player
	{
		// TODO: update to CString, needs a uint.Parse(CString) like option first
		internal string m_handle;

		internal Player()
		{
		}
		
		/// <summary>
		/// Gets the handle of this player
		/// </summary>
		public int Handle => int.Parse(m_handle);
		
		public static implicit operator CString(Player player) => player.m_handle;
		
		public static implicit operator string(Player player) => player.m_handle;

		public static implicit operator int(Player player) => player.Handle;
#else
	public abstract class Player : Core.Native.Input.Primitive
	{
		internal Player(ulong handle) : base(handle)
		{
		}
		

		/// <summary>
		/// Gets the handle of this player
		/// </summary>
		public int Handle => (int)m_nativeValue;
		
		public static implicit operator int(Player player) => player.Handle;
#endif
		
		/// <summary>
		/// Gets the name of this player
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Access the <see cref="StateBag"/> of this player
		/// </summary>
		public abstract StateBag State { get; }

		/// <summary>
		/// Gets the <see cref="IPed"/> this player is controling, use Ped.Character on Ped for non-shared access.
		/// </summary>
		public abstract IPed GetCharacter();
		
	}
}
